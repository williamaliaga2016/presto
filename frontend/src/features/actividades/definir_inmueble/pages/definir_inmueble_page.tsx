import { useEffect, useRef, useState } from 'react';
import { AxiosError } from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';
import { Toast } from 'primereact/toast';
import type { Toast as ToastRef } from 'primereact/toast';
import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from
  '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosTitularSection from
  '../../validar_informacion/components/DatosTitularSection';
import DatosCreditoSection from
  '../../validar_informacion/components/DatosCreditoSection';
import CondicionesFinancierasSection from
  '../../validar_informacion/components/CondicionesFinancierasSection';
import DefinirInmuebleSection from '../components/DefinirInmuebleSection';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from
  '../../validar_informacion/models/catalogo';
import {
  EMPTY_ENCABEZADO_VALIDAR_INFORMACION,
} from '../../validar_informacion/models/encabezado_validar_informacion';
import { EMPTY_VALIDAR_INFORMACION } from
  '../../validar_informacion/models/validar_informacion';
import type { ValidarInformacionBBVA } from
  '../../validar_informacion/models/validar_informacion';
import {
  DEFINIR_INMUEBLE_ACTIVITY_ID,
  EMPTY_DEFINIR_INMUEBLE,
  type DefinirInmuebleBBVA,
} from '../models/definir_inmueble';
import { useValidarInformacion } from '../../validar_informacion/hooks/useValidarInformacion';
import { useEncabezado } from '@/features/encabezado/hooks/useEncabezado';
import {
  useAvanzarDefinirInmueble,
  useControlesDefinirInmueble,
  useDefinirInmueble,
  useUpsertDefinirInmueble,
} from '../hooks/useDefinirInmueble';

type RequiredFieldsError = {
  detail?: string;
  message?: string;
  campos_faltantes?: string[];
};

function getRequiredFieldsFromError(error: unknown): string[] {
  const axiosError = error as AxiosError<RequiredFieldsError>;
  const data = axiosError.response?.data;

  if (Array.isArray(data?.campos_faltantes)) {
    return data.campos_faltantes;
  }

  const detail = data?.detail ?? data?.message ?? axiosError.message;
  const match = detail?.match(/Datos Obligatorios Faltantes:\s*(.+)$/i);
  if (match?.[1]) {
    return match[1]
      .split(',')
      .map((field) => field.trim())
      .filter(Boolean);
  }

  const lowerDetail = detail?.toLowerCase() ?? '';
  const fields: string[] = [];
  if (lowerDetail.includes('constructora')) fields.push('constructora');
  if (lowerDetail.includes('fecha estimada')) {
    fields.push('fecha_estimada_entrega');
  }

  return fields;
}

function formatFieldName(field: string): string {
  const names: Record<string, string> = {
    cliente_cuenta_inmueble_definido: 'Cliente tiene Inmueble Definido',
    constructora: 'Constructora',
    fecha_estimada_entrega: 'Fecha estimada de entrega',
    estatus_general: 'Estatus General',
    motivo_devolucion: 'Motivo de Devolucion',
    observaciones: 'Observaciones',
  };

  return names[field] ?? field.replaceAll('_', ' ');
}

function validateRequiredFields(form: DefinirInmuebleBBVA): Set<string> {
  const missing = new Set<string>();

  if (!form.estatus_general) missing.add('estatus_general');
  if (form.cliente_cuenta_inmueble_definido) {
    if (!form.constructora) missing.add('constructora');
    if (!form.fecha_estimada_entrega) {
      missing.add('fecha_estimada_entrega');
    }
  }

  if (
    (form.estatus_general === 'ESCALAR_COMERCIAL' ||
      form.estatus_general === 'CON_OBS') &&
    !form.motivo_devolucion
  ) {
    missing.add('motivo_devolucion');
  }

  return missing;
}

export default function DefinirInmueblePage() {
  const { id_expediente: idParam } = useParams();
  const navigate = useNavigate();
  const id_expediente = Number(idParam ?? 0);
  const toast = useRef<ToastRef>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [confirmVisible, setConfirmVisible] = useState(false);
  const [confirmMessage, setConfirmMessage] = useState('');
  const [invalidFields, setInvalidFields] = useState<Set<string>>(new Set());
  const [form, setForm] = useState<DefinirInmuebleBBVA>(
    EMPTY_DEFINIR_INMUEBLE(id_expediente),
  );

  const { data: actividadData, isLoading } =
    useDefinirInmueble(id_expediente);
  const { data: heredadosData } = useValidarInformacion(id_expediente);
  const { data: encabezadoData } = useEncabezado(
    id_expediente,
    DEFINIR_INMUEBLE_ACTIVITY_ID,
  );
  const { data: controlesData } = useControlesDefinirInmueble(id_expediente);
  const { mutate: guardar, isPending: isGuardando } = useUpsertDefinirInmueble();
  const { mutate: avanzar, isPending: isAvanzando } = useAvanzarDefinirInmueble();

  const encabezado =
    encabezadoData?.detail ?? EMPTY_ENCABEZADO_VALIDAR_INFORMACION;
  const heredados =
    heredadosData?.detail ?? EMPTY_VALIDAR_INFORMACION(id_expediente);
  const controles = controlesData?.detail ?? EMPTY_CONTROLES_VALIDAR_INFORMACION;

  const showRequiredFieldsToast = (missing: Set<string>) => {
    toast.current?.show({
      severity: 'warn',
      summary: 'Datos Obligatorios Faltantes',
      detail: Array.from(missing).map(formatFieldName).join(', '),
      life: 7000,
    });
  };

  const handleRequiredFieldsError = (error: unknown): boolean => {
    const backendMissing = getRequiredFieldsFromError(error);
    if (backendMissing.length === 0) return false;

    const missing = new Set(backendMissing);
    setInvalidFields(missing);
    showRequiredFieldsToast(missing);
    return true;
  };

  useEffect(() => {
    const formulario = actividadData?.detail;
    if (formulario) {
      // eslint-disable-next-line react-hooks/set-state-in-effect -- carga el borrador editable al recibir la consulta.
      setForm(formulario);
    }
  }, [actividadData]);

  const updateField = (field: keyof DefinirInmuebleBBVA, value: unknown) => {
    setForm((prev) => {
      const next = { ...prev, [field]: value };
      if (
        field === 'cliente_cuenta_inmueble_definido' &&
        value === true &&
        next.estatus_general === 'SIN_INM'
      ) {
        next.estatus_general = 'LISTO';
      }
      if (field === 'cliente_cuenta_inmueble_definido' && value !== true) {
        next.constructora = '';
        next.fecha_estimada_entrega = null;
      }
      return next;
    });
    if (!isEditing) setIsEditing(true);
    if (invalidFields.size > 0) {
      setInvalidFields((prev) => {
        const next = new Set(prev);
        next.delete(field as string);
        if (field === 'cliente_cuenta_inmueble_definido' && value !== true) {
          next.delete('constructora');
          next.delete('fecha_estimada_entrega');
        }
        if (
          field === 'estatus_general' &&
          value !== 'ESCALAR_COMERCIAL' &&
          value !== 'CON_OBS'
        ) {
          next.delete('motivo_devolucion');
        }
        return next;
      });
    }
  };

  const readOnlyChange = (_field: keyof ValidarInformacionBBVA, _value: unknown) =>
    undefined;

  const handleGuardar = () => {
    const missing = validateRequiredFields(form);
    if (missing.size > 0) {
      setInvalidFields(missing);
      showRequiredFieldsToast(missing);
      return;
    }

    setInvalidFields(new Set());
    guardar(form, {
      onSuccess: (response) => {
        setForm(response.detail);
        setIsEditing(false);
        setInvalidFields(new Set());
        toast.current?.show({
          severity: 'success',
          summary: 'Guardado',
          detail: 'Informacion guardada correctamente',
          life: 3000,
        });
      },
      onError: (error) => {
        if (handleRequiredFieldsError(error)) return;

        toast.current?.show({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
          life: 5000,
        });
      },
    });
  };

  const ejecutarAvance = (confirmar: boolean) => {
    avanzar(
      { id_expediente, confirmar },
      {
        onSuccess: (response) => {
          if (response.detail?.requiere_confirmacion) {
            setConfirmMessage(
              response.detail.mensaje ??
              'El inmueble aun no se encuentra definido, desea avanzar?',
            );
            setConfirmVisible(true);
            return;
          }

          setConfirmVisible(false);
          toast.current?.show({
            severity: 'success',
            summary: 'Avanzado',
            detail: 'Folio avanzado a la siguiente etapa',
            life: 3000,
          });
          navigate('/home/bandeja');
        },
        onError: (error) => {
          if (handleRequiredFieldsError(error)) return;

          toast.current?.show({
            severity: 'error',
            summary: 'Error al avanzar',
            detail: error.message,
            life: 5000,
          });
        },
      },
    );
  };

  const handleAvanzar = () => {
    const missing = validateRequiredFields(form);
    if (missing.size > 0) {
      setInvalidFields(missing);
      showRequiredFieldsToast(missing);
      return;
    }
    setInvalidFields(new Set());
    ejecutarAvance(false);
  };

  if (isLoading) return <div className="p-4">Cargando...</div>;

  return (
    <div className="flex flex-col gap-4 p-4">
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Definir Inmueble
      </h2>

      <Dialog
        header="Confirmar avance"
        visible={confirmVisible}
        modal
        className="w-[92vw] max-w-lg"
        onHide={() => setConfirmVisible(false)}
        footer={(
          <div className="flex justify-end gap-2">
            <Button
              label="Cancelar"
              icon="pi pi-times"
              severity="secondary"
              outlined
              onClick={() => setConfirmVisible(false)}
            />
            <Button
              label="Avanzar"
              icon="pi pi-arrow-right"
              loading={isAvanzando}
              onClick={() => ejecutarAvance(true)}
            />
          </div>
        )}
      >
        <p className="m-0 text-sm text-gray-700">{confirmMessage}</p>
      </Dialog>

      <div className="grid grid-cols-1 gap-4">
        <div>
          <Accordion multiple activeIndex={[0]}>
            <AccordionTab header="Informacion del Expediente">
              <EncabezadoActividad
                idExpediente={id_expediente}
                activityID={DEFINIR_INMUEBLE_ACTIVITY_ID}
              />
            </AccordionTab>
            <AccordionTab header="Funciones Transversales">
              <FuncionesTransversales
                idExpediente={id_expediente}
                idActividad={DEFINIR_INMUEBLE_ACTIVITY_ID}
              />
            </AccordionTab>
            <AccordionTab header="Datos Titular">
              <DatosTitularSection
                data={heredados}
                controles={controles}
                isEditing={false}
                onChange={readOnlyChange}
                invalidFields={new Set()}
              />
            </AccordionTab>

            <AccordionTab header="Datos del Credito">
              <DatosCreditoSection
                data={heredados}
                encabezado={encabezado}
                controles={controles}
                isEditing={false}
                onChange={readOnlyChange}
                invalidFields={new Set()}
              />
            </AccordionTab>

            <AccordionTab header="Condiciones Financieras">
              <CondicionesFinancierasSection
                data={heredados}
                encabezado={encabezado}
                isEditing={false}
                onChange={readOnlyChange}
                invalidFields={new Set()}
              />
            </AccordionTab>

            <AccordionTab header="Datos del Inmueble">
              <DefinirInmuebleSection
                data={form}
                invalidFields={invalidFields}
                onChange={updateField}
              />
            </AccordionTab>

            <AccordionTab header="Estatus General">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="flex flex-col gap-1">
                  <label className="text-xs text-gray-500">Estatus General</label>
                  <Dropdown
                    value={form.estatus_general}
                    options={controles.estatus_general}
                    optionLabel="description"
                    optionValue="code"
                    onChange={(e) => updateField('estatus_general', e.value)}
                    placeholder="Seleccionar..."
                    className={`w-full${
                      invalidFields.has('estatus_general') ? ' p-invalid' : ''
                    }`}
                  />
                </div>

                {(form.estatus_general === 'ESCALAR_COMERCIAL' ||
                  form.estatus_general === 'CON_OBS') && (
                    <div className="flex flex-col gap-1">
                      <label className="text-xs text-gray-500">
                        Motivo Devolucion
                      </label>
                      <Dropdown
                        value={form.motivo_devolucion}
                        options={controles.motivo_devolucion}
                        optionLabel="description"
                        optionValue="code"
                        onChange={(e) => updateField('motivo_devolucion', e.value)}
                        placeholder="Seleccionar..."
                        className={`w-full${
                          invalidFields.has('motivo_devolucion')
                            ? ' p-invalid'
                            : ''
                        }`}
                      />
                    </div>
                  )}

                <div className="flex flex-col gap-1 md:col-span-2">
                  <label className="text-xs text-gray-500">Observaciones</label>
                  <InputTextarea
                    value={form.observaciones ?? ''}
                    onChange={(e) => updateField('observaciones', e.target.value)}
                    rows={3}
                    className={`w-full${
                      invalidFields.has('observaciones') ? ' p-invalid' : ''
                    }`}
                  />
                </div>
              </div>
            </AccordionTab>
          </Accordion>

          <div className="flex justify-end gap-3 mt-4 pt-4 border-t">
            <Button
              label="Guardar"
              icon="pi pi-save"
              severity="secondary"
              loading={isGuardando}
              disabled={!isEditing}
              onClick={handleGuardar}
            />
            <Button
              label="Avanzar"
              icon="pi pi-arrow-right"
              loading={isAvanzando}
              disabled={isEditing}
              onClick={handleAvanzar}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
