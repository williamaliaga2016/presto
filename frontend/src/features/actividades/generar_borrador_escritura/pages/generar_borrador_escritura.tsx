import { useRef, useState, useMemo, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";

import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox } from "primereact/checkbox";
import { Toast } from "primereact/toast";
import { RadioButton } from "primereact/radiobutton";
import { Dropdown } from "primereact/dropdown";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { ProgressSpinner } from "primereact/progressspinner";

// Importar componentes comunes
import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';

// Importar hooks y servicios
import { useControlesGenerarBorradorEscritura } from '../hooks/useControlesGenerarBorradorEscritura';
import { useFiadoresGarantes } from '../hooks/useFiadoresGarantes';
import { useGenerarBorradorEscritura, useSaveGenerarBorradorEscritura } from '../hooks/useGenerarBorradorEscritura';
import type { CatalogoOption, ControlesGenerarBorradorEscritura } from '../models/catalogo';
import type { FiadorGarante, FiadorGaranteWithSelection } from '../models/generarBorradorEscritura';

// Constante de valores vacíos
const EMPTY_CONTROLES_GENERAR_BORRADOR: ControlesGenerarBorradorEscritura = {
  notaria: [],
  rolcomparecencia:[],
};

// Función para determinar el rol de comparecencia según los datos del fiador/garante
const determinarRolComparecencia = (fiador: FiadorGarante): number => {
  if (fiador.estado_civil === "002") return 4; // CÓNYUGE
  
  const nacionalidad = fiador.nacionalidad?.toUpperCase() || "";
  if (nacionalidad !== "CHILENA" && nacionalidad !== "CHILENA EN EL EXTERIOR" && nacionalidad !== "001") {
    return 5; // MANDATARIO
  }
  
  const relacionTitular = fiador.relacion_titular?.toUpperCase() || "";
  if (relacionTitular === "COMPRADOR" || relacionTitular === "001") return 1;
  if (relacionTitular === "VENDEDOR") return 2;
  if (relacionTitular === "FIADOR") return 3;
  
  return 1; // COMPRADOR por defecto
};

// Función para normalizar los datos del API con roles dinámicos y datos guardados
const normalizeComparecientes = (
  fiadores: FiadorGarante[], 
  detallesGuardados: any[] = []
): FiadorGaranteWithSelection[] => {
  const detallesMap = new Map();
  detallesGuardados.forEach((detalle) => {
    detallesMap.set(detalle.id_datos_operacion_fiador_garante, {
      id_generar_borrador_escritura_detalle_entity: detalle.id_generar_borrador_escritura_detalle_entity,
      id_rol_comparecencia: detalle.id_rol_comparecencia,
      requiere_firma: detalle.requiere_firma,
    });
  });

  return fiadores.map((fiador) => {
    const detalleGuardado = detallesMap.get(fiador.id_datos_operacion_fiador_garante);
    
    if (detalleGuardado) {
      return {
        ...fiador,
        id_generar_borrador_escritura_detalle_entity: detalleGuardado.id_generar_borrador_escritura_detalle_entity,
        id_rol_comparecencia: detalleGuardado.id_rol_comparecencia,
        requiere_firma: detalleGuardado.requiere_firma,
      };
    }
    
    return {
      ...fiador,
      id_generar_borrador_escritura_detalle_entity: 0,
      id_rol_comparecencia: determinarRolComparecencia(fiador),
      requiere_firma: false,
    };
  });
};

// Función para obtener string de beneficios
const getBeneficiosString = (form: any): string | null => {
  const beneficios = [];
  if (form.beneficio_dfl2) beneficios.push('DFL2');
  if (form.beneficio_vivienda_social) beneficios.push('Vivienda Social');
  if (form.beneficio_ds4) beneficios.push('DS4');
  return beneficios.length > 0 ? beneficios.join(',') : null;
};

// Función para cargar beneficios desde string
const loadBeneficiosFromString = (beneficiosStr: string | null | undefined) => {
  const beneficios = beneficiosStr?.split(',').map(b => b.trim()) || [];
  return {
    beneficio_dfl2: beneficios.includes('DFL2'),
    beneficio_vivienda_social: beneficios.includes('Vivienda Social'),
    beneficio_ds4: beneficios.includes('DS4'),
  };
};

/**
 * Actividad 5.20 Generar Borrador Escritura
 */
const ACTIVITY_ID = 'GENERAR_BORRADOR_ESCRITURA_ID'; // Reemplazar con el ID real

export default function GenerarBorradorEscrituraPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id } = useParams();
  const expedienteId = Number(id || 0);
  const hasValidExpediente = expedienteId > 0;

  // Estado sin rol_comparecencia_general (no se guarda en backend)
  const [form, setForm] = useState({
    existe_alzamiento: false,
    tiene_seguro_cesantia: false,
    mandatario_judicial: false,
    beneficio_dfl2: false,
    beneficio_vivienda_social: false,
    beneficio_ds4: false,
    id_notaria: null as number | null,
    enviar_reparo: false,
    observaciones: "",
  });

  // Estado local para rol comparecencia general (solo UI)
  const [rolComparecenciaGeneral, setRolComparecenciaGeneral] = useState<number | null>(null);

  const [comparecientes, setComparecientes] = useState<FiadorGaranteWithSelection[]>([]);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);

  // Hooks
  const controlesGenerarBorradorQuery = useControlesGenerarBorradorEscritura();
  const fiadoresGarantesQuery = useFiadoresGarantes(expedienteId, hasValidExpediente);
  const borradorQuery = useGenerarBorradorEscritura(expedienteId, hasValidExpediente);
  const saveMutation = useSaveGenerarBorradorEscritura();

  // Procesar catálogos de notarias
  const catalogos: ControlesGenerarBorradorEscritura = useMemo(() => {
    if (!controlesGenerarBorradorQuery.data?.status) {
      return EMPTY_CONTROLES_GENERAR_BORRADOR;
    }
    return controlesGenerarBorradorQuery.data.detail ?? EMPTY_CONTROLES_GENERAR_BORRADOR;
  }, [controlesGenerarBorradorQuery.data]);

  const notariasOptions = useMemo(() => {
    const notarias = catalogos.notaria || [];
    return notarias.map((notaria: CatalogoOption) => ({
      id: Number(notaria.code),
      descripcion: notaria.description,
    }));
  }, [catalogos.notaria]);

  // Opciones para roles de comparecencia - value como número
  const rolesComparecenciaOptions = useMemo(() => {
    const roles = catalogos.rolcomparecencia || [];
    return roles.map((rol: CatalogoOption) => ({
      label: rol.description || '',
      value: Number(rol.code),
    }));
  }, [catalogos.rolcomparecencia]);

  // Cargar datos guardados previamente (cabecera) - sin rol_comparecencia_general
  useEffect(() => {
    if (borradorQuery.data?.status && borradorQuery.data.detail) {
      const data = borradorQuery.data.detail;
      const beneficios = loadBeneficiosFromString(data.beneficios);
      
      setForm((prev) => ({
        ...prev,
        existe_alzamiento: data.existe_alzamiento,
        tiene_seguro_cesantia: data.seguro_cesantia,
        mandatario_judicial: data.mandato_judicial,
        ...beneficios,
        id_notaria: data.id_notaria ? Number(data.id_notaria) : null,
        enviar_reparo: data.enviar_reparo || false,
        observaciones: data.observaciones || "",
      }));
    }
  }, [borradorQuery.data]);

  // Cargar y normalizar los fiadores/garantes con los detalles guardados
  useEffect(() => {
    if (fiadoresGarantesQuery.data?.status && fiadoresGarantesQuery.data.detail) {
      const fiadores = fiadoresGarantesQuery.data.detail;
      const detallesGuardados = borradorQuery.data?.detail?.detalle || [];
      
      const comparecientesNormalizados = normalizeComparecientes(fiadores, detallesGuardados);
      setComparecientes(comparecientesNormalizados);
    }
  }, [fiadoresGarantesQuery.data, borradorQuery.data]);

  const isLoading = controlesGenerarBorradorQuery.isLoading || 
                    fiadoresGarantesQuery.isLoading || 
                    borradorQuery.isLoading;

  const updateField = (field: string, value: any) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleEditar = () => {
    if (!hasValidExpediente) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: 'El expediente es obligatorio.',
        life: 3000,
      });
      return;
    }
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    if (!hasValidExpediente) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: 'El expediente es obligatorio.',
        life: 3000,
      });
      return;
    }

    try {
      const payload = {
        id_generar_borrador_escritura: borradorQuery.data?.detail?.id_generar_borrador_escritura || 0,
        id_expediente: expedienteId,
        existe_alzamiento: form.existe_alzamiento,
        seguro_cesantia: form.tiene_seguro_cesantia,
        mandato_judicial: form.mandatario_judicial,
        beneficios: getBeneficiosString(form),
        id_notaria: form.id_notaria ? Number(form.id_notaria) : null,
        enviar_reparo: form.enviar_reparo,
        observaciones: form.observaciones,
        is_active: true,
        row_status: true,
        created_by: 0,
        created_date: new Date().toISOString(),
        modified_by: null,
        modified_date: null,
        detalle: comparecientes.map((f) => ({
          id_generar_borrador_escritura_detalle_entity: f.id_generar_borrador_escritura_detalle_entity || 0,
          id_generar_borrador_escritura: borradorQuery.data?.detail?.id_generar_borrador_escritura || 0,
          id_datos_operacion_fiador_garante: f.id_datos_operacion_fiador_garante,
          id_expediente: expedienteId,
          id_rol_comparecencia: f.id_rol_comparecencia || 1,
          requiere_firma: f.requiere_firma,
          is_active: true,
          row_status: true,
          created_by: 0,
          created_date: new Date().toISOString(),
          modified_by: null,
          modified_date: null,
        })),
      };

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Información guardada correctamente",
          life: 3000,
        });
        
        setIsDisabled(true);
        setCanAdvance(true);
        await borradorQuery.refetch();
      } else {
        toast.current?.show({
          severity: "error",
          summary: "Error",
          detail: response.message || "Error al guardar",
          life: 3000,
        });
      }
    } catch (error) {
      console.error('Error al guardar:', error);
      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: "Ocurrió un error al guardar",
        life: 3000,
      });
    }
  };

  const handleAvanzar = async () => {
    if (!hasValidExpediente) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: 'El expediente es obligatorio.',
        life: 3000,
      });
      return;
    }

    if (!canAdvance) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: 'Debe guardar los cambios antes de avanzar.',
        life: 3000,
      });
      return;
    }

    toast.current?.show({
      severity: 'info',
      summary: 'Avanzar',
      detail: 'Funcionalidad en desarrollo.',
      life: 3000,
    });
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  const handleFirmaChange = (rowData: FiadorGaranteWithSelection, checked: boolean) => {
    const updated = comparecientes.map((item) => {
      if (item.id_datos_operacion_fiador_garante === rowData.id_datos_operacion_fiador_garante) {
        return { ...item, requiere_firma: checked };
      }
      return item;
    });
    setComparecientes(updated);
  };

  const handleRolChange = (rowData: FiadorGaranteWithSelection, value: number) => {
    const updated = comparecientes.map((item) => {
      if (item.id_datos_operacion_fiador_garante === rowData.id_datos_operacion_fiador_garante) {
        return { ...item, id_rol_comparecencia: value };
      }
      return item;
    });
    setComparecientes(updated);
  };

  // OPTIMIZACIÓN 1: Memoizar el template del dropdown
  const rolComparecenciaBodyTemplate = useMemo(() => {
    return (rowData: FiadorGaranteWithSelection) => (
      <Dropdown
        value={rowData.id_rol_comparecencia}
        options={rolesComparecenciaOptions}
        optionLabel="label"
        optionValue="value"
        placeholder="Selecciona rol"
        className="w-full"
        onChange={(e) => handleRolChange(rowData, e.value)}
        disabled={isDisabled}
        filter
        showClear
      />
    );
  }, [isDisabled, rolesComparecenciaOptions]);

  // OPTIMIZACIÓN 2: Memoizar el template del checkbox
  const requiereFirmaBodyTemplate = useMemo(() => {
    return (rowData: FiadorGaranteWithSelection) => (
      <Checkbox
        checked={rowData.requiere_firma}
        onChange={(e) => handleFirmaChange(rowData, e.checked ?? false)}
        disabled={isDisabled}
      />
    );
  }, [isDisabled]);

  if (isLoading && hasValidExpediente) {
    return (
      <div className="flex justify-center items-center h-64">
        <ProgressSpinner />
      </div>
    );
  }

  return (
    <div>
      <Toast ref={toast} />

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información del Expediente" disabled={!hasValidExpediente}>
          <EncabezadoActividad 
            idExpediente={expedienteId} 
            activityID={ACTIVITY_ID} 
          />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!hasValidExpediente}>
          <FuncionesTransversales
            idExpediente={expedienteId}
            idActividad={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab header="Generar Borrador Escritura" disabled={!hasValidExpediente}>
          {!hasValidExpediente ? (
            <Card>
              <div className="text-center text-600">
                No se recibió un expediente válido para esta actividad.
              </div>
            </Card>
          ) : (
            <Card className="w-full shadow-md card-presto-form mb-6">
              {controlesGenerarBorradorQuery.isLoading && (
                <div className="mb-4 text-sm text-blue-600">
                  Cargando catálogos...
                </div>
              )}

              {fiadoresGarantesQuery.error && (
                <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                  Error al cargar los fiadores/garantes. Por favor, intente nuevamente.
                </div>
              )}

              <div className="grid grid-cols-1 md:grid-cols-4 gap-5">
                {/* EXISTE ALZAMIENTO */}
                <div className="flex flex-col gap-2">
                  <label className="font-semibold text-sm">Existe Alzamiento</label>
                  <div className="flex items-center gap-5 h-11">
                    <div className="flex items-center gap-2">
                      <RadioButton
                        inputId="alzamiento_si"
                        name="alzamiento"
                        value={true}
                        checked={form.existe_alzamiento === true}
                        onChange={(e) => updateField("existe_alzamiento", e.value)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="alzamiento_si">Sí</label>
                    </div>
                    <div className="flex items-center gap-2">
                      <RadioButton
                        inputId="alzamiento_no"
                        name="alzamiento"
                        value={false}
                        checked={form.existe_alzamiento === false}
                        onChange={(e) => updateField("existe_alzamiento", e.value)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="alzamiento_no">No</label>
                    </div>
                  </div>
                </div>

                {/* SEGURO CESANTIA */}
                <div className="flex flex-col gap-2">
                  <label className="font-semibold text-sm">Tiene Seguro de Cesantía</label>
                  <div className="flex items-center gap-5 h-11">
                    <div className="flex items-center gap-2">
                      <RadioButton
                        inputId="seguro_si"
                        name="seguro"
                        value={true}
                        checked={form.tiene_seguro_cesantia === true}
                        onChange={(e) => updateField("tiene_seguro_cesantia", e.value)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="seguro_si">Sí</label>
                    </div>
                    <div className="flex items-center gap-2">
                      <RadioButton
                        inputId="seguro_no"
                        name="seguro"
                        value={false}
                        checked={form.tiene_seguro_cesantia === false}
                        onChange={(e) => updateField("tiene_seguro_cesantia", e.value)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="seguro_no">No</label>
                    </div>
                  </div>
                </div>

                {/* MANDATARIO JUDICIAL */}
                <div className="flex flex-col gap-2">
                  <label className="font-semibold text-sm">Mandatario Judicial</label>
                  <div className="flex items-center gap-5 h-11">
                    <div className="flex items-center gap-2">
                      <RadioButton
                        inputId="mandatario_si"
                        name="mandatario"
                        value={true}
                        checked={form.mandatario_judicial === true}
                        onChange={(e) => updateField("mandatario_judicial", e.value)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="mandatario_si">Sí</label>
                    </div>
                    <div className="flex items-center gap-2">
                      <RadioButton
                        inputId="mandatario_no"
                        name="mandatario"
                        value={false}
                        checked={form.mandatario_judicial === false}
                        onChange={(e) => updateField("mandatario_judicial", e.value)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="mandatario_no">No</label>
                    </div>
                  </div>
                </div>

                {/* BENEFICIOS */}
                <div className="flex flex-col gap-2 md:col-span-2">
                  <label className="font-semibold text-sm">Beneficios</label>
                  <div className="flex flex-wrap items-center gap-5 min-h-[44px]">
                    <div className="flex items-center gap-2">
                      <Checkbox
                        inputId="beneficio_dfl2"
                        checked={form.beneficio_dfl2}
                        onChange={(e) => updateField("beneficio_dfl2", e.checked ?? false)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="beneficio_dfl2">DFL2</label>
                    </div>
                    <div className="flex items-center gap-2">
                      <Checkbox
                        inputId="beneficio_vivienda_social"
                        checked={form.beneficio_vivienda_social}
                        onChange={(e) => updateField("beneficio_vivienda_social", e.checked ?? false)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="beneficio_vivienda_social">Vivienda Social</label>
                    </div>
                    <div className="flex items-center gap-2">
                      <Checkbox
                        inputId="beneficio_ds4"
                        checked={form.beneficio_ds4}
                        onChange={(e) => updateField("beneficio_ds4", e.checked ?? false)}
                        disabled={isDisabled}
                      />
                      <label htmlFor="beneficio_ds4">DS4</label>
                    </div>
                  </div>
                </div>

                {/* ASIGNACION NOTARIA */}
                <div className="flex flex-col gap-1 md:col-span-2">
                  <label className="font-semibold text-sm">Asignación de la Notaría</label>
                  <Dropdown
                    value={form.id_notaria ? Number(form.id_notaria) : null}
                    options={notariasOptions}
                    optionLabel="descripcion"
                    optionValue="id"
                    filter
                    showClear
                    placeholder={notariasOptions.length === 0 ? "Cargando notarias..." : "--- Selecciona ---"}
                    className="w-full"
                    onChange={(e) => updateField("id_notaria", e.value ? Number(e.value) : null)}
                    disabled={controlesGenerarBorradorQuery.isLoading || isDisabled}
                  />
                </div>

                {/* ROL COMPARECENCIA GENERAL - Solo UI, no se guarda */}
                <div className="flex flex-col gap-1 md:col-span-2">
                  <label className="font-semibold text-sm">Rol Comparecencia General</label>
                  <Dropdown
                    value={rolComparecenciaGeneral}
                    options={rolesComparecenciaOptions}
                    optionLabel="label"
                    optionValue="value"
                    placeholder={rolesComparecenciaOptions.length === 0 ? "Cargando roles..." : "--- Selecciona ---"}
                    className="w-full"
                    onChange={(e) => setRolComparecenciaGeneral(e.value)}
                    disabled={controlesGenerarBorradorQuery.isLoading || isDisabled}
                    showClear
                    filter
                  />
                </div>

                {/* TABLA DE COMPARECIENTES/FIADORES - OPTIMIZADA */}
                <div className="md:col-span-4">
                  <DataTable
                    key={isDisabled ? "disabled" : "enabled"}
                    value={comparecientes}
                    responsiveLayout="scroll"
                    className="p-datatable-sm"
                    emptyMessage={fiadoresGarantesQuery.isLoading ? "Cargando fiadores..." : "No existen fiadores/garantes registrados"}
                  >
                    <Column 
                      header="Rol Comparecencia"
                      body={rolComparecenciaBodyTemplate}
                      style={{ width: "200px" }}
                    />
                    <Column field="rut" header="Rut" />
                    <Column field="nombres" header="Nombres" />
                    <Column field="apellido_paterno" header="Apellido Paterno" />
                    <Column field="apellido_materno" header="Apellido Materno" />
                    <Column field="telefono_movil" header="Teléfono" />
                    <Column field="email" header="Email" />
                    <Column
                      header="Requiere Firma"
                      body={requiereFirmaBodyTemplate}
                      style={{ width: "120px", textAlign: "center" }}
                    />
                  </DataTable>
                </div>
              </div>

              {/* ESTATUS DE LA ACTIVIDAD */}
              <div className="md:col-span-4 mt-6">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Estatus de la Actividad
                </div>
                <div className="flex items-center gap-3">
                  <Checkbox
                    inputId="enviar_reparo"
                    checked={form.enviar_reparo}
                    onChange={(e) => updateField("enviar_reparo", e.checked ?? false)}
                    disabled={isDisabled}
                  />
                  <label htmlFor="enviar_reparo" className="font-semibold text-sm">
                    ¿Enviar a Reparo?
                  </label>
                </div>
              </div>

              {/* OBSERVACIONES */}
              <div className="md:col-span-4 mt-4">
                <div className="bg-blue-600 text-white font-bold text-sm px-4 py-2 uppercase rounded mb-4">
                  Observaciones
                </div>
                <textarea
                  id="observaciones"
                  value={form.observaciones || ""}
                  onChange={(e) => updateField("observaciones", e.target.value)}
                  rows={3}
                  className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
                  placeholder="Escriba aquí sus observaciones..."
                  disabled={isDisabled}
                />
              </div>

              {/* BOTONES */}
              <div className="form-actions mt-6 flex gap-3">
                <Button
                  type="button"
                  label="Editar"
                  icon="pi pi-pencil"
                  severity="info"
                  outlined
                  disabled={!isDisabled}
                  onClick={handleEditar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label={saveMutation.isPending ? "Guardando..." : "Guardar"}
                  icon="pi pi-save"
                  severity="success"
                  disabled={isDisabled}
                  onClick={handleGuardar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label="Avanzar"
                  icon="pi pi-arrow-right"
                  severity="warning"
                  disabled={!canAdvance}
                  onClick={handleAvanzar}
                  className="btn-responsive"
                />

                <Button
                  type="button"
                  label="Salir"
                  icon="pi pi-sign-out"
                  severity="secondary"
                  outlined
                  onClick={handleSalir}
                  className="btn-responsive"
                />
              </div>
            </Card>
          )}
        </AccordionTab>
      </Accordion>
    </div>
  );
}