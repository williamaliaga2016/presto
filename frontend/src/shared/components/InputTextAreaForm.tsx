import { InputTextarea } from 'primereact/inputtextarea';

interface InputTextAreaFormProps {
  label: string;
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  maxLength?: number;
  rows?: number;
  disabled?: boolean;
  required?: boolean;
  showCounter?: boolean;
}

export default function InputTextAreaForm({
  label,
  value,
  onChange,
  placeholder,
  maxLength = 500,
  rows = 3,
  disabled = false,
  required = false,
  showCounter = true,
}: InputTextAreaFormProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
        {label} {required && '*'}
      </label>
      <InputTextarea
        value={value}
        onChange={(e) => onChange(e.target.value.slice(0, maxLength))}
        rows={rows}
        maxLength={maxLength}
        className="form-input-presto w-full"
        placeholder={placeholder}
        disabled={disabled}
      />
      {showCounter && (
        <span className="text-xs text-gray-400 text-right">
          {value.length}/{maxLength}
        </span>
      )}
    </div>
  );
}
