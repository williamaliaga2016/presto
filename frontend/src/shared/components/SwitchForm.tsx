import { InputSwitch } from 'primereact/inputswitch';

interface Props {
  label: string;
  value: boolean | null;
  onChange: (value: boolean) => void;
  disabled?: boolean;
  required?: boolean;
  hint?: string;
}

/**
 * Componente switch reutilizable con label.
 * Convierte SI/NO a booleano internamente.
 */
export default function SwitchForm({
  label,
  value,
  onChange,
  disabled = false,
  required = false,
  hint,
}: Props) {
  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
        {label} {required && '*'}
      </label>
      <div className="flex items-center gap-3">
        <InputSwitch
          checked={value ?? false}
          onChange={(e) => onChange(e.value ?? false)}
          disabled={disabled}
        />
        <span className="text-sm text-gray-700">
          {value ? 'SÍ' : 'NO'}
        </span>
      </div>
      {hint && (
        <span className="text-xs text-orange-600">{hint}</span>
      )}
    </div>
  );
}
