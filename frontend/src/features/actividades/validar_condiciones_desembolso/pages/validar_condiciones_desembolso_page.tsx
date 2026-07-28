import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';
import { Badge } from 'primereact/badge';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosHeredadosSection from '../components/DatosHeredadosSection';
import CondicionesDesembolsoSection from '../components/CondicionesDesembolsoSection';
import type { ValidarCondicionesDesembolso } from '../models/validar_condiciones_desembolso';
import { EMPTY_VALIDAR_CONDICIONES_DESEMBOLSO } from '../models/validar_condiciones_desembolso';
import { useValidarCondicionesDesembolso } from '../hooks/useValidarCondicionesDesembolso';
import { useUpsertValidarCondicionesDesembolso } from '../hooks/useUpsertValidarCondicionesDesembolso';
import { useAvanzarValidarCondicionesDesembolso } from '../hooks/useAvanzarValidarCondicionesDesembolso';
import { useSuspenderValidarCondicionesDesembolso } from '../hooks/useSuspenderValidarCondicionesDesembolso';

const ACTIVITY_ID = 'BBVA_ESCRITURACION_VALIDAR_CONDICIONES_DESEMBOLSO';

export default function ValidarCondicionesDesembolsoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<ValidarCondicionesDesembolso>(EMPTY_VALIDAR_CONDICIONES_DESEMBOLSO(id_expediente));
  const [datosHeredados, setDatosHeredados] = useState<any>(null);
  const [puedeSuspender, setPuedeSuspender] = useState(false);
  const [conteoCaidas, setConteoCaidas] = useState(0);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useValidarCondicionesDesembolso(id_expediente);
  const saveMutation = useUpsertValidarCondicionesDesembolso();
  const avanzarMutation = useAvanzarValidarCondicionesDesembolso();
  const suspenderMutation = useSuspenderValidarCondicionesDesembolso(id_expediente);

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(EMPTY_VALIDAR_CONDICIONES_DESEMBOLSO(id_expediente));
      setIsDisabled(true); setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;
    if (data?.status && data.detail) {
      const { formulario, puede_suspender, conteo_caidas } = data.detail;
      const loaded: ValidarCondicionesDesembolso = { ...EMPTY_VALIDAR_CONDICIONES_DESEMBOLSO(id_expediente), ...formulario, id_expediente };
      setForm(loaded);
      setPuedeSuspender(puede_suspender);
      setConteoCaidas(conteo_caidas);
      setDatosHeredados((data.detail as any).datos_heredados ?? null);
      setIsDisabled(Number(loaded.id) > 0);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    } else if (data) { hasHydratedRef.current = true; }
  }, [data, id_expediente]);

  const updateField = <K extends keyof ValidarCondicionesDesembolso>(field: K, value: ValidarCondicionesDesembolso[K]) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEditar = () => { setIsDisabled(false); setCanAdvance(false); };

  const handleSuspender = async () => {
    try {
      setIsBusy(true);
      const response = await suspenderMutation.mutateAsync();
      if (response.status) {
        toast.current?.show({ severity: 'info', summary: 'Suspendida', detail: 'Actividad suspendida. No se contabilizarán tiempos de ANS.', life: 4000 });
      }
    } catch {
      toast.current?.show({ severity: 'error', summary: 'Error', detail: 'No se pudo suspender.', life: 3000 });
    } finally { setIsBusy(false); }
  };

  const handleGuardar = async () => {
    if (!form.id_expediente || form.id_expediente <= 0) { toast.current?.show({ severity: 'warn', summary: 'Validación', detail: 'No existe un id_expediente válido.', life: 3000 }); return; }
    try {
      setIsBusy(true);
      const payload: ValidarCondicionesDesembolso = { ...form, id: Number(form.id ?? 0), id_expediente: Number(form.id_expediente || id_expediente || 0), id_actividad: ACTIVITY_ID, is_active: true, row_status: true };
      const response = await saveMutation.mutateAsync(payload);
      if (response.status) {
        toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Información guardada correctamente', life: 3000 });
        setForm({ ...EMPTY_VALIDAR_CONDICIONES_DESEMBOLSO(id_expediente), ...(response.detail ?? payload) });
        setIsDisabled(true); setCanAdvance(true); hasHydratedRef.current = true;
      } else { toast.current?.show({ severity: 'warn', summary: 'Atención', detail: response.message || 'No se pudo guardar', life: 3000 }); }
    } catch { toast.current?.show({ severity: 'error', summary: 'Error', detail: 'Ocurrió un error al guardar', life: 3000 }); }
    finally { setIsBusy(false); }
  };

  const validateAvanzar = (): string[] => {
    const m: string[] = [];
    if (!form.confirmar_plan_pagos) m.push('Confirmar verificación del Plan de Pagos');
    if (!form.requiere_escalamiento_comercial) m.push('¿Requiere Escalamiento Comercial?');
    return m;
  };

  const handleAvanzar = async () => {
    const camposFaltantes = validateAvanzar();
    if (camposFaltantes.length > 0) { toast.current?.show({ severity: 'warn', summary: 'Validación', detail: `Campos obligatorios faltantes: ${camposFaltantes.join(', ')}`, life: 5000 }); return; }
    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(Number(form.id_expediente ?? 0));
      if (response.status) { toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Actividad avanzada correctamente', life: 2000 }); navigate('/home/bandeja'); }
      else { toast.current?.show({ severity: 'warn', summary: 'Atención', detail: response.message || 'No se pudo avanzar.', life: 3000 }); }
    } catch { toast.current?.show({ severity: 'error', summary: 'Error', detail: 'Ocurrió un error al avanzar.', life: 3000 }); }
    finally { setIsBusy(false); }
  };

  return (
    <>
      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Validar Condiciones Desembolso</h2>
      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab disabled={!id_expediente || id_expediente <= 0} header="Información del Expediente">
          <EncabezadoActividad idExpediente={Number(form.id_expediente || id_expediente || 0)} activityID={ACTIVITY_ID} />
        </AccordionTab>
        <AccordionTab header="Funciones Transversales" disabled={!id_expediente || id_expediente <= 0}>
          <FuncionesTransversales idExpediente={Number(form.id_expediente || id_expediente || 0)} idActividad={ACTIVITY_ID} show_registro_contacto={false} />
        </AccordionTab>
        <AccordionTab header="Validar Condiciones Desembolso">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && <div className="mb-4 text-sm text-blue-600">Cargando información...</div>}

            {/* Datos Heredados (CA02) */}
            <div className="mb-4">
              <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Información Heredada</h3>
              <DatosHeredadosSection datosHeredados={datosHeredados} />
            </div>

            <div className="border-t border-slate-200 my-4" />

            {/* Botón Suspender + Badge Caídas */}
            <div className="flex items-center justify-between mb-4">
              <div className="flex items-center gap-2">
                {conteoCaidas > 0 && (
                  <span className="text-sm text-gray-600">
                    Caídas registradas: <Badge value={conteoCaidas.toString()} severity="warning" />
                  </span>
                )}
              </div>
              {puedeSuspender && (
                <Button
                  type="button"
                  label="Suspender Actividad"
                  icon="pi pi-pause-circle"
                  severity="danger"
                  outlined
                  onClick={handleSuspender}
                  disabled={isBusy || form.suspendida}
                  size="small"
                />
              )}
            </div>

            {form.suspendida && (
              <div className="mb-4 p-3 bg-yellow-50 border border-yellow-200 rounded text-sm text-yellow-800">
                <i className="pi pi-exclamation-triangle mr-2" />
                Esta actividad se encuentra suspendida. Los tiempos de ANS no se están contabilizando.
              </div>
            )}

            <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Condiciones de Desembolso</h3>
            <CondicionesDesembolsoSection form={form} isDisabled={isDisabled} updateField={updateField} />

            <div className="form-actions">
              <Button type="button" label="Editar" icon="pi pi-pencil" severity="info" outlined onClick={handleEditar} disabled={isBusy || !isDisabled} className="btn-responsive" />
              <Button type="button" label={saveMutation.isPending ? 'Guardando...' : 'Guardar'} icon="pi pi-save" severity="success" onClick={handleGuardar} disabled={isBusy || isDisabled} className="btn-responsive" />
              <Button type="button" label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'} icon="pi pi-arrow-right" severity="warning" onClick={handleAvanzar} disabled={isBusy || !canAdvance} className="btn-responsive" />
              <Button type="button" label="Salir" icon="pi pi-sign-out" severity="secondary" outlined onClick={() => navigate('/home/bandeja')} disabled={isBusy} className="btn-responsive" />
            </div>
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
