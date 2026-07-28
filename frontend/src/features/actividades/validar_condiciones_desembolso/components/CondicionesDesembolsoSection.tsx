import { InputTextarea } from 'primereact/inputtextarea';
import SwitchForm from '@/shared/components/SwitchForm';
import type { ValidarCondicionesDesembolso } from '../models/validar_condiciones_desembolso';

interface Props {
  form: ValidarCondicionesDesembolso;
  isDisabled: boolean;
  updateField: <K extends keyof ValidarCondicionesDesembolso>(field: K, value: ValidarCondicionesDesembolso[K]) => void;
}

export default function CondicionesDesembolsoSection({ form, isDisabled, updateField }: Props) {
  return (
    <div className="space-y-4">
      {/* Confirmar Plan de Pagos */}
      <SwitchForm
        label="Confirmar verificación del Plan de Pagos"
        value={form.confirmar_plan_pagos}
        onChange={(val) => updateField('confirmar_plan_pagos', val)}
        disabled={isDisabled}
        required
      />

      {/* ¿Requiere Escalamiento Comercial? */}
      <SwitchForm
        label="¿Requiere Escalamiento Comercial?"
        value={form.requiere_escalamiento_comercial === 'SI'}
        onChange={(val) => updateField('requiere_escalamiento_comercial', val ? 'SI' : 'NO')}
        disabled={isDisabled}
        required
      />

      {/* Observaciones */}
      <div className="flex flex-col gap-1.5">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
          Observaciones
        </label>
        <InputTextarea
          value={form.observaciones ?? ''}
          onChange={(e) => updateField('observaciones', e.target.value || null)}
          disabled={isDisabled}
          rows={3}
          placeholder="Observaciones opcionales..."
        />
      </div>
    </div>
  );
}
