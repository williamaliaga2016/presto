import { Calendar } from 'primereact/calendar';
import { Checkbox } from 'primereact/checkbox';
import { InputText } from 'primereact/inputtext';
import type { DefinirInmuebleBBVA } from '../models/definir_inmueble';

type Props = {
  data: DefinirInmuebleBBVA;
  invalidFields: Set<string>;
  onChange: (field: keyof DefinirInmuebleBBVA, value: unknown) => void;
};

function parseDate(value?: string | null): Date | null {
  if (!value) return null;
  const [year, month, day] = value.substring(0, 10).split('-').map(Number);
  if (!year || !month || !day) return null;
  return new Date(year, month - 1, day);
}

function toIsoDate(value: Date | null): string | null {
  if (!value) return null;
  const year = value.getFullYear();
  const month = String(value.getMonth() + 1).padStart(2, '0');
  const day = String(value.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

export default function DefinirInmuebleSection({
  data,
  invalidFields,
  onChange,
}: Props) {
  const inmuebleDefinido = data.cliente_cuenta_inmueble_definido ?? false;
  const inv = (field: string) => invalidFields.has(field);

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div className="flex items-center gap-2 md:col-span-2">
        <Checkbox
          checked={inmuebleDefinido}
          onChange={(e) =>
            onChange('cliente_cuenta_inmueble_definido', e.checked)}
          inputId="cliente_cuenta_inmueble_definido"
        />
        <label
          htmlFor="cliente_cuenta_inmueble_definido"
          className="text-sm font-medium"
        >
          ¿Cliente tiene Inmueble Definido?
        </label>
      </div>

      {inmuebleDefinido && (
        <>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Constructora</label>
            <InputText
              value={data.constructora ?? ''}
              onChange={(e) => onChange('constructora', e.target.value)}
              aria-invalid={inv('constructora')}
              className={`w-full${inv('constructora') ? ' p-invalid' : ''}`}
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">
              Fecha estimada de entrega
            </label>
            <Calendar
              value={parseDate(data.fecha_estimada_entrega)}
              onChange={(e) =>
                onChange(
                  'fecha_estimada_entrega',
                  toIsoDate(e.value as Date | null),
                )}
              showIcon
              dateFormat="dd/mm/yy"
              className={`w-full${
                inv('fecha_estimada_entrega') ? ' p-invalid' : ''
              }`}
              inputClassName={`w-full${
                inv('fecha_estimada_entrega') ? ' p-invalid' : ''
              }`}
            />
          </div>
        </>
      )}
    </div>
  );
}
