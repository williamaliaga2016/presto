import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosHeredadosSection from '../components/DatosHeredadosSection';
import VurSection from '../components/VurSection';
import RecepcionBoletaSection from '../components/RecepcionBoletaSection';
import type { RealizarRecepcionBoleta, DatosHeredadosRecepcionBoleta } from '../models/realizar_recepcion_boleta';
import { EMPTY_REALIZAR_RECEPCION_BOLETA } from '../models/realizar_recepcion_boleta';
import { EMPTY_CONTROLES_RECEPCION_BOLETA } from '../models/controles';
import { useRealizarRecepcionBoleta } from '../hooks/useRealizarRecepcionBoleta';
import { useControlesRecepcionBoleta } from '../hooks/useControlesRecepcionBoleta';
import { useUpsertRealizarRecepcionBoleta } from '../hooks/useUpsertRealizarRecepcionBoleta';
import { useAvanzarRealizarRecepcionBoleta } from '../hooks/useAvanzarRealizarRecepcionBoleta';
import { useEjecutarVUR } from '../hooks/useEjecutarVUR';

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA';

export default function RealizarRecepcionBoletaPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RealizarRecepcionBoleta>(EMPTY_REALIZAR_RECEPCION_BOLETA(id_expediente));
  const [datosHeredados, setDatosHeredados] = useState<DatosHeredadosRecepcionBoleta | null>(null);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRealizarRecepcionBoleta(id_expediente);
  const { data: controlesData } = useControlesRecepcionBoleta();
  const saveMutation = useUpsertRealizarRecepcionBoleta();
  const avanzarMutation = useAvanzarRealizarRecepcionBoleta();
  const vurMutation = useEjecutarVUR(id_expediente);

  const controles = controlesData?.detail ?? EMPTY_CONTROLES_RECEPCION_BOLETA;

  // --- Hydration Effects ---

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(EMPTY_REALIZAR_RECEPCION_BOLETA(id_expediente));
      setDatosHeredados(null);
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const { formulario, datos_heredados } = data.detail;

      const loaded: RealizarRecepcionBoleta = {
        ...EMPTY_REALIZAR_RECEPCION_BOLETA(id_expediente),
        ...formulario,
        id_expediente,
      };
      setForm(loaded);
      setDatosHeredados(datos_heredados ?? null);
      setIsDisabled(Number(loaded.id) > 0);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  // --- Handlers ---

  const updateField = <K extends keyof RealizarRecepcionBoleta>(
    field: K,
    value: RealizarRecepcionBoleta[K],
  ) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEjecutarVUR = async () => {
    try {
      setIsBusy(true);
      const response = await vurMutation.mutateAsync();
      if (response.status && response.detail) {
        const updated = response.detail;
        setForm((prev) => ({
          ...prev,
          numero_boleta: updated.numero_boleta ?? prev.numero_boleta,
          fecha_boleta: updated.fecha_boleta ?? prev.fecha_boleta,
          numero_matricula: updated.numero_matricula ?? prev.numero_matricula,
          oficina_registro: updated.oficina_registro ?? prev.oficina_registro,
          vur_ejecutado: updated.vur_ejecutado,
          vur_exitoso: updated.vur_exitoso,
          vur_intentos: updated.vur_intentos,
        }));

        toast.current?.show({
          severity: updated.vur_exitoso ? 'success' : 'warn',
          summary: updated.vur_exitoso ? 'VUR Exitoso' : 'VUR Fallido',
          detail: response.message,
          life: 4000,
        });
      }
    } catch {
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Error al ejecutar el VUR.',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleEditar = () => {
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    if (!form.id_expediente || form.id_expediente <= 0) {
      toast.current?.show({ severity: 'warn', summary: 'Validación', detail: 'No existe un id_expediente válido.', life: 3000 });
      return;
    }

    try {
      setIsBusy(true);
      const payload: RealizarRecepcionBoleta = {
        ...form,
        id: Number(form.id ?? 0),
        id_expediente: Number(form.id_expediente || id_expediente || 0),
        id_actividad: ACTIVITY_ID,
        is_active: true,
        row_status: true,
      };

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Información guardada correctamente', life: 3000 });
        const saved: RealizarRecepcionBoleta = { ...EMPTY_REALIZAR_RECEPCION_BOLETA(id_expediente), ...(response.detail ?? payload) };
        setForm(saved);
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({ severity: 'warn', summary: 'Atención', detail: response.message || 'No se pudo guardar', life: 3000 });
      }
    } catch {
      toast.current?.show({ severity: 'error', summary: 'Error', detail: 'Ocurrió un error al guardar', life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const validateAvanzar = (): string[] => {
    const missing: string[] = [];
    if (!form.numero_boleta?.trim()) missing.push('Número de Boleta');
    if (!form.fecha_boleta) missing.push('Fecha de Boleta');
    if (form.fecha_boleta && new Date(form.fecha_boleta) > new Date()) missing.push('Fecha de Boleta (no puede ser futura)');
    if (!form.numero_matricula?.trim()) missing.push('Número de Matrícula');
    if (!form.tipo_boleta) missing.push('Tipo de Boleta');
    if (!form.codigo_zona?.trim()) missing.push('Código Zona');
    if (!form.oficina_registro) missing.push('Oficina de Registro');
    if (!form.boleta_recibida) missing.push('Boleta Recibida');
    if ((form.tipo_boleta === 'TBOL-2' || form.tipo_boleta === 'TBOL-3') && !form.boleta_en_poder_de?.trim()) {
      missing.push('Boleta En Poder De');
    }
    return missing;
  };

  const handleAvanzar = async () => {
    const camposFaltantes = validateAvanzar();
    if (camposFaltantes.length > 0) {
      toast.current?.show({ severity: 'warn', summary: 'Validación', detail: `Campos obligatorios faltantes: ${camposFaltantes.join(', ')}`, life: 5000 });
      return;
    }

    const expedienteId = Number(form.id_expediente ?? 0);
    if (!expedienteId || expedienteId <= 0) {
      toast.current?.show({ severity: 'warn', summary: 'Validación', detail: 'No existe un id_expediente válido para avanzar.', life: 3000 });
      return;
    }

    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(expedienteId);
      if (response.status) {
        toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Actividad avanzada correctamente', life: 2000 });
        navigate('/home/bandeja');
      } else {
        toast.current?.show({ severity: 'warn', summary: 'Atención', detail: response.message || 'No se pudo avanzar la actividad.', life: 3000 });
      }
    } catch {
      toast.current?.show({ severity: 'error', summary: 'Error', detail: 'Ocurrió un error al avanzar.', life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate('/home/bandeja');

  // --- Render ---

  return (
    <>
      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Realizar Recepción Boleta</h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab disabled={!id_expediente || id_expediente <= 0} header="Información del Expediente">
          <EncabezadoActividad idExpediente={Number(form.id_expediente || id_expediente || 0)} activityID={ACTIVITY_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!id_expediente || id_expediente <= 0}>
          <FuncionesTransversales idExpediente={Number(form.id_expediente || id_expediente || 0)} idActividad={ACTIVITY_ID} show_registro_contacto={false} />
        </AccordionTab>

        <AccordionTab header="Realizar Recepción Boleta">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">Cargando información...</div>
            )}

            {/* Datos Heredados (solo lectura) */}
            <div className="mb-6">
              <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Datos Heredados</h3>
              <DatosHeredadosSection datosHeredados={datosHeredados} />
            </div>

            <div className="border-t border-slate-200 my-4" />

            {/* Sección VUR */}
            <div className="mb-6">
              <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Extracción VUR</h3>
              <VurSection
                form={form}
                isDisabled={isDisabled}
                updateField={updateField}
                onEjecutarVUR={handleEjecutarVUR}
                isVurLoading={vurMutation.isPending}
              />
            </div>

            <div className="border-t border-slate-200 my-4" />

            {/* Campos de Recepción */}
            <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Datos de Recepción</h3>
            <RecepcionBoletaSection
              form={form}
              isDisabled={isDisabled}
              updateField={updateField}
              tipoBoletaOptions={controles.tipo_boleta}
              oficinaOptions={controles.oficina_registro}
            />

            {/* Botones de acción */}
            <div className="form-actions">
              <Button type="button" label="Editar" icon="pi pi-pencil" severity="info" outlined onClick={handleEditar} disabled={isBusy || !isDisabled} className="btn-responsive" />
              <Button type="button" label={saveMutation.isPending ? 'Guardando...' : 'Guardar'} icon="pi pi-save" severity="success" onClick={handleGuardar} disabled={isBusy || isDisabled} className="btn-responsive" />
              <Button type="button" label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'} icon="pi pi-arrow-right" severity="warning" onClick={handleAvanzar} disabled={isBusy || !canAdvance} className="btn-responsive" />
              <Button type="button" label="Salir" icon="pi pi-sign-out" severity="secondary" outlined onClick={handleSalir} disabled={isBusy} className="btn-responsive" />
            </div>
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
