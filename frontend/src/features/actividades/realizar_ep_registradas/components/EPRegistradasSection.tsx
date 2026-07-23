import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { Calendar } from 'primereact/calendar';
import { InputSwitch } from 'primereact/inputswitch';
import type { RealizarEPRegistradas } from '../models/realizar_ep_registradas';

interface Props {
  form: RealizarEPRegistradas;
  isDisabled: boolean;
  updateField: <K extends keyof RealizarEPRegistradas>(field: K, value: RealizarEPRegistradas[K]) => void;
}

export default function EPRegistradasSection({ form, isDisabled, updateField }: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-4">
      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-700">Finalización *</label>
        <Calendar
          value={form.finalizacion ? new Date(form.finalizacion) : null}
          onChange={(e) => updateField('finalizacion', e.value ? (e.value as Date).toISOString().split('T')[0] : null)}
          disabled={isDisabled} dateFormat="yy-mm-dd" showIcon placeholder="AAAA-MM-DD"
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-700">Causal *</label>
        <InputText
          value={form.causal ?? ''} onChange={(e) => updateField('causal', e.target.value || null)}
          disabled={isDisabled} placeholder="Causal..."
        />
      </div>

      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-700">Fecha Finalización *</label>
        <Calendar
          value={form.fecha_finalizacion ? new Date(form.fecha_finalizacion) : null}
          onChange={(e) => updateField('fecha_finalizacion', e.value ? (e.value as Date).toISOString().split('T')[0] : null)}
          disabled={isDisabled} dateFormat="yy-mm-dd" showIcon placeholder="AAAA-MM-DD"
        />
      </div>

      <div className="flex flex-col gap-1 justify-center">
        <label className="text-xs font-medium text-gray-700">Confirmación de EP Registrada *</label>
        <InputSwitch
          checked={form.confirmacion_ep_registrada}
          onChange={(e) => updateField('confirmacion_ep_registrada', e.value ?? false)}
          disabled={isDisabled}
        />
      </div>

      <div className="flex flex-col gap-1 md:col-span-2">
        <label className="text-xs font-medium text-gray-700">Observaciones</label>
        <InputTextarea
          value={form.observaciones ?? ''} onChange={(e) => updateField('observaciones', e.target.value || null)}
          disabled={isDisabled} rows={3} placeholder="Observaciones opcionales..."
        />
      </div>
    </div>
  );
}
