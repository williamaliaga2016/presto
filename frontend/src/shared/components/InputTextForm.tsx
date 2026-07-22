import { InputText } from 'primereact/inputtext';

interface InputTextFormProps {
  label: string;
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  maxLength?: number;
  disabled?: boolean;
  required?: boolean;
}

export default function InputTextForm({
  label,
  value,
  onChange,
  placeholder,
  maxLength,
  disabled = false,
  required = false,
}: InputTextFormProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
        {label} {required && '*'}
      </label>
      <InputText
        value={value}
        onChange={(e) => onChange(e.target.value)}
        maxLength={maxLength}
        className="form-input-presto w-full"
        placeholder={placeholder}
        disabled={disabled}
      />
    </div>
  );
}
