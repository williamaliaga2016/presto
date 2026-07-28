import { Calendar } from 'primereact/calendar';

interface InputTimeFormProps {
  label: string;
  /** Valor en formato "HH:mm", o null si no hay hora seleccionada. */
  value: string | null;
  onChange: (value: string | null) => void;
  placeholder?: string;
  disabled?: boolean;
  required?: boolean;
}

function parseHHmm(value: string | null): Date | null {
  if (!value) return null;
  const [hours, minutes] = value.split(':').map(Number);
  if (Number.isNaN(hours) || Number.isNaN(minutes)) return null;

  const date = new Date();
  date.setHours(hours, minutes, 0, 0);
  return date;
}

function formatHHmm(date: Date): string {
  const hh = String(date.getHours()).padStart(2, '0');
  const mm = String(date.getMinutes()).padStart(2, '0');
  return `${hh}:${mm}`;
}

export default function InputTimeForm({
  label,
  value,
  onChange,
  placeholder = 'HH:MM',
  disabled = false,
  required = false,
}: InputTimeFormProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
        {label} {required && '*'}
      </label>
      <Calendar
        value={parseHHmm(value)}
        onChange={(e) => {
          if (e.value instanceof Date) {
            onChange(formatHHmm(e.value));
          } else {
            onChange(null);
          }
        }}
        timeOnly
        hourFormat="24"
        showIcon
        icon="pi pi-clock"
        placeholder={placeholder}
        className="w-full"
        inputClassName="form-input-presto w-full"
        disabled={disabled}
      />
    </div>
  );
}
