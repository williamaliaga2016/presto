import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosHeredadosSection from '../components/DatosHeredadosSection';
import VBFinalSection from '../components/VBFinalSection';
import type { RealizarVBFinalAbogado } from '../models/realizar_vb_final_abogado';
import { EMPTY_REALIZAR_VB_FINAL_ABOGADO } from '../models/realizar_vb_final_abogado';
import { EMPTY_CONTROLES_VB_FINAL } from '../models/controles';
import { useRealizarVBFinalAbogado } from '../hooks/useRealizarVBFinalAbogado';
import { useControlesVBFinalAbogado } from '../hooks/useControlesVBFinalAbogado';
import { useUpsertRealizarVBFinalAbogado } from '../hooks/useUpsertRealizarVBFinalAbogado';
import { useAvanzarRealizarVBFinalAbogado } from '../hooks/useAvanzarRealizarVBFinalAbogado';

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO';

export default function RealizarVBFinalAbogadoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RealizarVBFinalAbogado>(EMPTY_REALIZAR_VB_FINAL_ABOGADO(id_expediente));
  const [datosHeredados, setDatosHeredados] = useState<any>(null);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRealizarVBFinalAbogado(id_expediente);
  const { data: controlesData } = useControlesVBFinalAbogado();
  const saveMutation = useUpsertRealizarVBFinalAbogado();
  const avanzarMutation = useAvanzarRealizarVBFinalAbogado();

  const controles = controlesData?.detail ?? EMPTY_CONTROLES_VB_FINAL;
  const tipologiaOptions = controles.tipologia ?? [];
  const casuisticaOptions = controles.casuistica ?? [];

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(EMPTY_REALIZAR_VB_FINAL_ABOGADO(id_expediente));
      setIsDisabled(true); setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;
    if (data?.status && data.detail) {
      const { formulario } = data.detail;
      const loaded: RealizarVBFinalAbogado = { ...EMPTY_REALIZAR_VB_FINAL_ABOGADO(id_expediente), ...formulario, id_expediente };
      if (!loaded.requiere_devolucion) loaded.requiere_devolucion = 'NO';
      setForm(loaded); setIsDisabled(Number(loaded.id) > 0); setCanAdvance(false); hasHydratedRef.current = true;
      setDatosHeredados((data.detail as any).datos_heredados ?? null);
    } else if (data) { hasHydratedRef.current = true; }
  }, [data, id_expediente]);

  const updateField = <K extends keyof RealizarVBFinalAbogado>(field: K, value: RealizarVBFinalAbogado[K]) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleRequiereDevolucionChange = (value: string) => {
    if (value === 'NO') {
      setForm((f) => ({
        ...f,
        requiere_devolucion: value,
        tipologia: null,
        casuistica: null,
        observaciones: null,
      }));
    } else {
      setForm((f) => ({ ...f, requiere_devolucion: value }));
    }
  };

  const handleTipologiaChange = (value: string) => {
    setForm((f) => ({ ...f, tipologia: value }));
  };

  const handleEditar = () => { setIsDisabled(false); setCanAdvance(false); };

  const handleGuardar = async () => {
    if (!form.id_expediente || form.id_expediente <= 0) { toast.current?.show({ severity: 'warn', summary: 'Validación', detail: 'No existe un id_expediente válido.', life: 3000 }); return; }
    try {
      setIsBusy(true);
      const payload: RealizarVBFinalAbogado = { ...form, id: Number(form.id ?? 0), id_expediente: Number(form.id_expediente || id_expediente || 0), id_actividad: ACTIVITY_ID, is_active: true, row_status: true };
      const response = await saveMutation.mutateAsync(payload);
      if (response.status) {
        toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Información guardada correctamente', life: 3000 });
        setForm({ ...EMPTY_REALIZAR_VB_FINAL_ABOGADO(id_expediente), ...(response.detail ?? payload) });
        setIsDisabled(true); setCanAdvance(true); hasHydratedRef.current = true;
      } else { toast.current?.show({ severity: 'warn', summary: 'Atención', detail: response.message || 'No se pudo guardar', life: 3000 }); }
    } catch { toast.current?.show({ severity: 'error', summary: 'Error', detail: 'Ocurrió un error al guardar', life: 3000 }); }
    finally { setIsBusy(false); }
  };

  const validateAvanzar = (): string[] => {
    const m: string[] = [];
    if (!form.requiere_devolucion) m.push('¿Requiere Devolución?');
    if (form.requiere_devolucion === 'SI') {
      if (!form.tipologia) m.push('Tipología');
      if (!form.casuistica) m.push('Casuística');
      if (!form.observaciones?.trim()) m.push('Observaciones');
    }
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
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Realizar VB Final Abogado</h2>
      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab disabled={!id_expediente || id_expediente <= 0} header="Información del Expediente">
          <EncabezadoActividad idExpediente={Number(form.id_expediente || id_expediente || 0)} activityID={ACTIVITY_ID} />
        </AccordionTab>
        <AccordionTab header="Funciones Transversales" disabled={!id_expediente || id_expediente <= 0}>
          <FuncionesTransversales idExpediente={Number(form.id_expediente || id_expediente || 0)} idActividad={ACTIVITY_ID} show_registro_contacto={false} />
        </AccordionTab>
        <AccordionTab header="Realizar VB Final Abogado">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && <div className="mb-4 text-sm text-blue-600">Cargando información...</div>}

            {/* Datos Heredados (CA02) */}
            <div className="mb-6">
              <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Datos Heredados</h3>
              <DatosHeredadosSection datosHeredados={datosHeredados} origenTramite={form.origen_tramite} />
            </div>

            <div className="border-t border-slate-200 my-4" />

            <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Visto Bueno</h3>
            <VBFinalSection
              form={form}
              isDisabled={isDisabled}
              updateField={updateField}
              tipologiaOptions={tipologiaOptions}
              casuisticaOptions={casuisticaOptions}
              onRequiereDevolucionChange={handleRequiereDevolucionChange}
              onTipologiaChange={handleTipologiaChange}
            />

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
