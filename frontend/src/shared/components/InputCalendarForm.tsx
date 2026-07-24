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
        onChange={(e) => {
          if (e.value instanceof Date) {
            const d = e.value;
            const yyyy = d.getFullYear();
            const mm = String(d.getMonth() + 1).padStart(2, '0');
            const dd = String(d.getDate()).padStart(2, '0');
            onChange(`${yyyy}-${mm}-${dd}`);
          } else {
            onChange(null);
          }
        }}
        showIcon
        dateFormat={dateFormat}
        placeholder={placeholder}
        className="w-full"
        inputClassName="form-input-presto w-full"
        disabled={disabled}
      />
    </div>
  );
}
