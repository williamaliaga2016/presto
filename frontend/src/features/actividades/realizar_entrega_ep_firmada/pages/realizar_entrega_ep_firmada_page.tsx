import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosHeredadosSection from '../components/DatosHeredadosSection';
import EntregaEpSection from '../components/EntregaEpSection';
import type { RealizarEntregaEpFirmada, DatosHeredadosEntregaEp } from '../models/realizar_entrega_ep_firmada';
import { EMPTY_REALIZAR_ENTREGA_EP_FIRMADA } from '../models/realizar_entrega_ep_firmada';
import { useRealizarEntregaEpFirmada } from '../hooks/useRealizarEntregaEpFirmada';
import { useUpsertRealizarEntregaEpFirmada } from '../hooks/useUpsertRealizarEntregaEpFirmada';
import { useAvanzarRealizarEntregaEpFirmada } from '../hooks/useAvanzarRealizarEntregaEpFirmada';

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA';

export default function RealizarEntregaEpFirmadaPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RealizarEntregaEpFirmada>(EMPTY_REALIZAR_ENTREGA_EP_FIRMADA(id_expediente));
  const [datosHeredados, setDatosHeredados] = useState<DatosHeredadosEntregaEp | null>(null);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRealizarEntregaEpFirmada(id_expediente);
  const saveMutation = useUpsertRealizarEntregaEpFirmada();
  const avanzarMutation = useAvanzarRealizarEntregaEpFirmada();

  // --- Hydration Effects ---

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(EMPTY_REALIZAR_ENTREGA_EP_FIRMADA(id_expediente));
      setDatosHeredados(null);
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const { formulario, datos_heredados } = data.detail;

      const loaded: RealizarEntregaEpFirmada = {
        ...EMPTY_REALIZAR_ENTREGA_EP_FIRMADA(id_expediente),
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

  const updateField = <K extends keyof RealizarEntregaEpFirmada>(
    field: K,
    value: RealizarEntregaEpFirmada[K],
  ) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEditar = () => {
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    if (!form.id_expediente || form.id_expediente <= 0) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: 'No existe un id_expediente válido.',
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const payload: RealizarEntregaEpFirmada = {
        ...form,
        id: Number(form.id ?? 0),
        id_expediente: Number(form.id_expediente || id_expediente || 0),
        id_actividad: ACTIVITY_ID,
        is_active: true,
        row_status: true,
      };

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Información guardada correctamente',
          life: 3000,
        });

        const saved: RealizarEntregaEpFirmada = {
          ...EMPTY_REALIZAR_ENTREGA_EP_FIRMADA(id_expediente),
          ...(response.detail ?? payload),
        };
        setForm(saved);
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No se pudo guardar',
          life: 3000,
        });
      }
    } catch {
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Ocurrió un error al guardar',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const validateAvanzar = (): string[] => {
    const missing: string[] = [];
    if (!form.entregado_a?.trim()) missing.push('Entregado a');
    return missing;
  };

  const handleAvanzar = async () => {
    const camposFaltantes = validateAvanzar();
    if (camposFaltantes.length > 0) {
      const msg = `Campos obligatorios faltantes: ${camposFaltantes.join(', ')}`;
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: msg,
        life: 5000,
      });
      return;
    }

    const expedienteId = Number(form.id_expediente ?? 0);
    if (!expedienteId || expedienteId <= 0) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: 'No existe un id_expediente válido para avanzar.',
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(expedienteId);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Actividad avanzada correctamente',
          life: 2000,
        });
        navigate('/home/bandeja');
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No se pudo avanzar la actividad.',
          life: 3000,
        });
      }
    } catch {
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Ocurrió un error al avanzar.',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  // --- Render ---

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Realizar Entrega EP Firmada
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab
          disabled={!id_expediente || id_expediente <= 0}
          header="Información del Expediente"
        >
          <EncabezadoActividad
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            activityID={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            idActividad={ACTIVITY_ID}
            show_registro_contacto={false}
          />
        </AccordionTab>

        <AccordionTab header="Realizar Entrega EP Firmada">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">
                Cargando información...
              </div>
            )}

            {/* Datos Heredados (solo lectura) */}
            <div className="mb-6">
              <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                Datos Heredados — Firmar Rep. Legal / Escritura
              </h3>
              <DatosHeredadosSection datosHeredados={datosHeredados} />
            </div>

            <div className="border-t border-slate-200 my-4" />

            {/* Datos de Entrega (editable) */}
            <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
              Datos de Entrega
            </h3>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-4">
              <EntregaEpSection
                form={form}
                isDisabled={isDisabled}
                updateField={updateField}
              />
            </div>

            {/* Botones de acción */}
            <div className="form-actions">
              <Button
                type="button"
                label="Editar"
                icon="pi pi-pencil"
                severity="info"
                outlined
                onClick={handleEditar}
                disabled={isBusy || !isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={saveMutation.isPending ? 'Guardando...' : 'Guardar'}
                icon="pi pi-save"
                severity="success"
                onClick={handleGuardar}
                disabled={isBusy || isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={isBusy || !canAdvance}
                className="btn-responsive"
              />

              <Button
                type="button"
                label="Salir"
                icon="pi pi-sign-out"
                severity="secondary"
                outlined
                onClick={handleSalir}
                disabled={isBusy}
                className="btn-responsive"
              />
            </div>
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
