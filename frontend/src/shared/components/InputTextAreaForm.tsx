import { InputTextarea } from 'primereact/inputtextarea';

interface InputTextAreaFormProps {
  id?: string;
  label: string;
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  maxLength?: number;
  rows?: number;
  disabled?: boolean;
  required?: boolean;
  showCounter?: boolean;
  /** Cuando true, resalta el campo como inválido (borde rojo + aria-invalid). */
  invalid?: boolean;
  /** Mensaje de error inline, mostrado bajo el campo solo cuando `invalid` es true. */
  errorMessage?: string;
}

export default function InputTextAreaForm({
  id,
  label,
  value,
  onChange,
  placeholder,
  maxLength = 500,
  rows = 3,
  disabled = false,
  required = false,
  showCounter = true,
  invalid = false,
  errorMessage,
}: InputTextAreaFormProps) {
  const errorId = id && errorMessage ? `${id}-error` : undefined;
  const showError = invalid && !!errorMessage;

  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700" htmlFor={id}>
        {label} {required && '*'}
      </label>
      <InputTextarea
        id={id}
        value={value}
        onChange={(e) => onChange(e.target.value.slice(0, maxLength))}
        rows={rows}
        maxLength={maxLength}
        className={`form-input-presto w-full${invalid ? ' p-invalid' : ''}`}
        placeholder={placeholder}
        disabled={disabled}
        aria-required={required}
        aria-invalid={invalid}
        aria-describedby={errorId}
      />
      {(showError || showCounter) && (
        <div className="flex items-start justify-between gap-2">
          {showError ? (
            <span id={errorId} role="alert" className="text-xs text-red-600">
              {errorMessage}
            </span>
          ) : (
            <span />
          )}
          {showCounter && (
            <span className="text-xs text-gray-400 whitespace-nowrap">
              {value.length}/{maxLength}
            </span>
          )}
        </div>
      )}
    </div>
  );
}
