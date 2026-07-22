import { Dropdown } from 'primereact/dropdown';
import type { CatalogoOption } from '@/models/CatalogoOption';

interface DropdownFormProps {
  label: string;
  value: string | null;
  options: CatalogoOption[];
  onChange: (value: string | null) => void;
  placeholder?: string;
  disabled?: boolean;
  required?: boolean;
  filter?: boolean;
  filterPlaceholder?: string;
}

export default function DropdownForm({
  label,
  value,
  options,
  onChange,
  placeholder = 'Seleccionar...',
  disabled = false,
  required = false,
  filter = false,
  filterPlaceholder = 'Buscar...',
}: DropdownFormProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
        {label} {required && '*'}
      </label>
      <Dropdown
        value={value}
        options={options}
        optionLabel="description"
        optionValue="code"
        onChange={(e) => onChange(e.value ?? null)}
        placeholder={placeholder}
        className="w-full"
        disabled={disabled || options.length === 0}
        filter={filter}
        filterPlaceholder={filterPlaceholder}
      />
    </div>
  );
}
