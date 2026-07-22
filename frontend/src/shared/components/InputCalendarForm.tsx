import { Calendar } from 'primereact/calendar';
import { normalizeDate, toDateValue } from '@/shared/utils/dateUtils';

interface InputCalendarFormProps {
  label: string;
  value: string | null;
  onChange: (value: string | null) => void;
  placeholder?: string;
  dateFormat?: string;
  disabled?: boolean;
  required?: boolean;
}

export default function InputCalendarForm({
  label,
  value,
  onChange,
  placeholder = 'dd/mm/aaaa',
  dateFormat = 'dd/mm/yy',
  disabled = false,
  required = false,
}: InputCalendarFormProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
        {label} {required && '*'}
      </label>
      <Calendar
        value={toDateValue(value)}
        onChange={(e) =>
          onChange(normalizeDate(e.value instanceof Date ? e.value.toISOString() : null))
        }
        showIcon
        dateFormat={dateFormat}
        placeholder={placeholder}
        className="form-input-presto w-full"
        disabled={disabled}
      />
    </div>
  );
}
