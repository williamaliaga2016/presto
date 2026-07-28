import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import SwitchForm from '@/shared/components/SwitchForm';
import type { RealizarDevolucionEP, DictamenPrevio } from '../models/realizar_devolucion_ep';
import { EMPTY_REALIZAR_DEVOLUCION_EP, ACCIONES_A_SEGUIR } from '../models/realizar_devolucion_ep';
import { useRealizarDevolucionEP } from '../hooks/useRealizarDevolucionEP';
import { useUpsertRealizarDevolucionEP } from '../hooks/useUpsertRealizarDevolucionEP';
import { useAvanzarRealizarDevolucionEP } from '../hooks/useAvanzarRealizarDevolucionEP';
import { useControlesDevolucionEP } from '../hooks/useControlesDevolucionEP';

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REALIZAR_DEVOLUCION_EP';

export default function RealizarDevolucionEPPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RealizarDevolucionEP>(EMPTY_REALIZAR_DEVOLUCION_EP(id_expediente));
  const [dictamenesPrevios, setDictamenesPrevios] = useState<DictamenPrevio[]>([]);
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRealizarDevolucionEP(id_expediente);
  const { data: controlesData } = useControlesDevolucionEP();
  const saveMutation = useUpsertRealizarDevolucionEP();
  const avanzarMutation = useAvanzarRealizarDevolucionEP();

  const tipologiaOptions = controlesData?.detail?.tipologia ?? [];

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      setForm(EMPTY_REALIZAR_DEVOLUCION_EP(id_expediente)); setDictamenesPrevios([]); setIsDisabled(true); setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (!data?.status || !data.detail) return;
    const { formulario, dictamenes_previos } = data.detail;

    const loaded: RealizarDevolucionEP = { ...EMPTY_REALIZAR_DEVOLUCION_EP(id_expediente), ...formulario, id_expediente };
    if (!loaded.requiere_escalamiento_comercial) loaded.requiere_escalamiento_comercial = 'NO';
    setForm(loaded);
    setDictamenesPrevios(dictamenes_previos ?? []);
    setIsDisabled(Number(loaded.id) > 0);
  }, [data, id_expediente]);

  const updateField = <K extends keyof RealizarDevolucionEP>(field: K, value: RealizarDevolucionEP[K]) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEscalamientoChange = (val: boolean) => {
    setForm((f) => ({
      ...f,
      requiere_escalamiento_comercial: val ? 'SI' : 'NO',
      tipologia: val ? f.tipologia : null,
      casuistica: val ? f.casuistica : null,
      accion_a_seguir: val ? null : f.accion_a_seguir,
    }));
  };

  const handleEditar = () => { setIsDisabled(false); setCanAdvance(false); };

  const handleGuardar = async () => {
    if (!form.id_expediente || form.id_expediente <= 0) { toast.current?.show({ severity: 'warn', summary: 'Validación', detail: 'No existe un id_expediente válido.', life: 3000 }); return; }
    try {
      setIsBusy(true);
      const payload: RealizarDevolucionEP = { ...form, id: Number(form.id ?? 0), id_expediente: Number(form.id_expediente || id_expediente || 0), id_actividad: ACTIVITY_ID, is_active: true, row_status: true };
      const response = await saveMutation.mutateAsync(payload);
      if (response.status) {
        toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Información guardada correctamente', life: 3000 });
        setForm({ ...EMPTY_REALIZAR_DEVOLUCION_EP(id_expediente), ...(response.detail ?? payload) });
        setIsDisabled(true); setCanAdvance(true);
      } else { toast.current?.show({ severity: 'warn', summary: 'Atención', detail: response.message || 'No se pudo guardar', life: 3000 }); }
    } catch { toast.current?.show({ severity: 'error', summary: 'Error', detail: 'Ocurrió un error al guardar', life: 3000 }); }
    finally { setIsBusy(false); }
  };

  const validateAvanzar = (): string[] => {
    const m: string[] = [];
    if (!form.requiere_escalamiento_comercial) m.push('¿Requiere Escalamiento Comercial?');
    if (form.requiere_escalamiento_comercial === 'SI' && !form.tipologia) m.push('Tipología');
    if (form.requiere_escalamiento_comercial === 'NO' && !form.accion_a_seguir) m.push('Acción a Seguir');
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

  const esEscalamiento = form.requiere_escalamiento_comercial === 'SI';

  /** Renderiza "—" cuando el valor es null/vacío */
  const emptyBody = (rowData: DictamenPrevio, field: keyof DictamenPrevio) => {
    const val = rowData[field];
    return val ? <span>{val}</span> : <span className="text-gray-400">—</span>;
  };

  return (
    <>
      <Toast ref={toast} />
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Realizar Devolución EP</h2>
      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab disabled={!id_expediente || id_expediente <= 0} header="Información del Expediente">
          <EncabezadoActividad idExpediente={Number(form.id_expediente || id_expediente || 0)} activityID={ACTIVITY_ID} />
        </AccordionTab>
        <AccordionTab header="Funciones Transversales" disabled={!id_expediente || id_expediente <= 0}>
          <FuncionesTransversales idExpediente={Number(form.id_expediente || id_expediente || 0)} idActividad={ACTIVITY_ID} show_registro_contacto={false} />
        </AccordionTab>
        <AccordionTab header="Realizar Devolución EP">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && <div className="mb-4 text-sm text-blue-600">Cargando información...</div>}

            {/* Grilla de Conceptos / Dictámenes Previos */}
            <div className="mb-6">
              <h4 className="text-xs font-bold text-blue-700 uppercase tracking-wide mb-2 flex items-center gap-1">
                <i className="pi pi-table text-xs" /> Conceptos / Dictámenes Previos
              </h4>
              <DataTable value={dictamenesPrevios} size="small" stripedRows className="text-sm" emptyMessage="Sin dictámenes registrados">
                <Column field="area" header="Área" style={{ width: '15%' }} body={(row: DictamenPrevio) => <span className="font-medium text-blue-600">{row.area}</span>} />
                <Column field="tipologia" header="Tipología" style={{ width: '25%' }} body={(row: DictamenPrevio) => emptyBody(row, 'tipologia')} />
                <Column field="casuistica" header="Casuística de Rechazo" style={{ width: '30%' }} body={(row: DictamenPrevio) => emptyBody(row, 'casuistica')} />
                <Column field="observaciones" header="Observaciones" style={{ width: '30%' }} body={(row: DictamenPrevio) => emptyBody(row, 'observaciones')} />
              </DataTable>
            </div>

            <div className="border-t border-slate-200 my-4" />
            <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">Decisión de Enrutamiento</h3>

            <div className="space-y-4">
              {/* Toggle escalamiento */}
              <SwitchForm
                label="¿Requiere Escalamiento Comercial?"
                value={form.requiere_escalamiento_comercial === 'SI'}
                onChange={handleEscalamientoChange}
                disabled={isDisabled}
                required
              />

              {/* Campos condicionales — mutuamente excluyentes (CA06) */}
              {esEscalamiento && (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="flex flex-col gap-1.5">
                    <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Tipología *</label>
                    <Dropdown value={form.tipologia} options={tipologiaOptions} optionLabel="description" optionValue="code" onChange={(e) => updateField('tipologia', e.value)} disabled={isDisabled} placeholder="Seleccione..." />
                  </div>
                </div>
              )}

              {!esEscalamiento && form.requiere_escalamiento_comercial === 'NO' && (
                <div className="flex flex-col gap-1.5">
                  <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Acción a Seguir *</label>
                  <Dropdown value={form.accion_a_seguir} options={ACCIONES_A_SEGUIR} optionLabel="description" optionValue="code" onChange={(e) => updateField('accion_a_seguir', e.value)} disabled={isDisabled} placeholder="Seleccione..." />
                </div>
              )}

              {/* Observaciones */}
              <div className="flex flex-col gap-1.5">
                <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Observaciones</label>
                <InputTextarea value={form.observaciones ?? ''} onChange={(e) => updateField('observaciones', e.target.value || null)} disabled={isDisabled} rows={3} placeholder="Justifique las acciones para solventar la devolución..." />
              </div>
            </div>

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
