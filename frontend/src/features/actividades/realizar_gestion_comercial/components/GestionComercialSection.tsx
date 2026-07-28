import { InputTextarea } from 'primereact/inputtextarea';
import SwitchForm from '@/shared/components/SwitchForm';
import type { RealizarGestionComercial } from '../models/realizar_gestion_comercial';

interface Props {
  form: RealizarGestionComercial;
  isDisabled: boolean;
  updateField: <K extends keyof RealizarGestionComercial>(field: K, value: RealizarGestionComercial[K]) => void;
}

export default function GestionComercialSection({ form, isDisabled, updateField }: Props) {
  return (
    <div className="space-y-4">
      <SwitchForm
        label="¿Cliente Desiste del Caso?"
        value={form.cliente_desiste === 'SI'}
        onChange={(val) => updateField('cliente_desiste', val ? 'SI' : 'NO')}
        disabled={isDisabled}
        required
      />

      <div className="flex flex-col gap-1.5">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
          Observaciones Comerciales
        </label>
        <InputTextarea
          value={form.observaciones ?? ''}
          onChange={(e) => updateField('observaciones', e.target.value || null)}
          disabled={isDisabled}
          rows={3}
          placeholder="Justifique acuerdos con el cliente o razones del desistimiento..."
        />
      </div>
    </div>
  );
}
