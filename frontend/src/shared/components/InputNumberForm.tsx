import { InputNumber } from 'primereact/inputnumber';

interface InputNumberFormProps {
  label: string;
  value: number | null;
  onChange: (value: number | null) => void;
  placeholder?: string;
  min?: number;
  max?: number;
  disabled?: boolean;
  required?: boolean;
  useGrouping?: boolean;
}

export default function InputNumberForm({
  label,
  value,
  onChange,
  placeholder,
  min,
  max,
  disabled = false,
  required = false,
  useGrouping = false,
}: InputNumberFormProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
        {label} {required && '*'}
      </label>
      <InputNumber
        value={value}
        onChange={(e) => onChange(e.value ?? null)}
        min={min}
        max={max}
        useGrouping={useGrouping}
        className="form-input-presto w-full"
        placeholder={placeholder}
        disabled={disabled}
      />
    </div>
  );
}
