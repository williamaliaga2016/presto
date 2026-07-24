import InputTextForm from '@/shared/components/InputTextForm';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import InputCalendarForm from '@/shared/components/InputCalendarForm';
import SwitchForm from '@/shared/components/SwitchForm';
import type { RealizarEPRegistradas } from '../models/realizar_ep_registradas';

interface Props {
  form: RealizarEPRegistradas;
  isDisabled: boolean;
  updateField: <K extends keyof RealizarEPRegistradas>(field: K, value: RealizarEPRegistradas[K]) => void;
}

export default function EPRegistradasSection({ form, isDisabled, updateField }: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-4">
      <InputCalendarForm
        label="Finalización"
        value={form.finalizacion}
        onChange={(val) => updateField('finalizacion', val)}
        disabled={isDisabled}
        required
      />

      <InputTextForm
        label="Causal"
        value={form.causal ?? ''}
        onChange={(val) => updateField('causal', val || null)}
        disabled={isDisabled}
        placeholder="Causal..."
        required
      />

      <InputCalendarForm
        label="Fecha Finalización"
        value={form.fecha_finalizacion}
        onChange={(val) => updateField('fecha_finalizacion', val)}
        disabled={isDisabled}
        required
      />

      <SwitchForm
        label="Confirmación de EP Registrada"
        value={form.confirmacion_ep_registrada}
        onChange={(val) => updateField('confirmacion_ep_registrada', val)}
        disabled={isDisabled}
        required
      />

      <div className="md:col-span-2">
        <InputTextAreaForm
          label="Observaciones"
          value={form.observaciones ?? ''}
          onChange={(val) => updateField('observaciones', val || null)}
          disabled={isDisabled}
          rows={3}
          placeholder="Observaciones opcionales..."
        />
      </div>
    </div>
  );
}
