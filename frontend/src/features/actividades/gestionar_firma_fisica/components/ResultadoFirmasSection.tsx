import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { Calendar } from 'primereact/calendar';

import type { GestionarFirmaFisica } from '../models/gestionar_firma_fisica';
import type { CatalogoOption } from '../models/controles';
import { useDropdownTooltip } from '@/shared/hooks/useDropdownTooltip';

interface Props {
  formulario: GestionarFirmaFisica;
  isDisabled: boolean;
  updateField: <K extends keyof GestionarFirmaFisica>(
    field: K,
    value: GestionarFirmaFisica[K],
  ) => void;
  resultadoGestoriaOptions: CatalogoOption[];
}

const normalizeDate = (value?: string | null): string | null => {
  if (!value) return null;

  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return null;
  }

  const yyyy = parsed.getFullYear();
  const mm = String(parsed.getMonth() + 1).padStart(2, '0');
  const dd = String(parsed.getDate()).padStart(2, '0');

  return `${yyyy}-${mm}-${dd}`;
};

const toDateValue = (value?: string | null): Date | null => {
  if (!value) return null;

  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? null : parsed;
};

export default function ResultadoFirmasSection({
  formulario,
  isDisabled,
  updateField,
  resultadoGestoriaOptions,
}: Props) {
  const { itemTemplate, valueTemplate } = useDropdownTooltip('description');

  return (
    <div className="flex flex-col gap-4 mt-6">
      <h4 className="text-blue-800 font-bold tracking-wider border-b pb-1">
        Resultado de Firmas
      </h4>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-4">
        <div className="flex flex-col gap-1.5">
          <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
            Motorizado Asignado *
          </label>
          <InputText
            value={formulario.motorizado_asignado ?? ''}
            onChange={(e) => updateField('motorizado_asignado', e.target.value)}
            className="form-input-presto w-full"
            placeholder="Nombre del motorizado..."
            disabled={isDisabled}
          />
        </div>

        <div className="flex flex-col gap-1.5">
          <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
            Fecha de Gestoría *
          </label>
          <Calendar
            value={toDateValue(formulario.fecha_gestoria)}
            onChange={(e) =>
              updateField(
                'fecha_gestoria',
                normalizeDate(
                  e.value instanceof Date ? e.value.toISOString() : null,
                ),
              )
            }
            showIcon
            dateFormat="dd/mm/yy"
            placeholder="dd/mm/aaaa"
            className="form-input-presto w-full"
            disabled={isDisabled}
          />
        </div>

        <div className="flex flex-col gap-1.5">
          <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
            Resultado de Gestoría *
          </label>
          <Dropdown
            value={formulario.resultado_gestoria}
            options={resultadoGestoriaOptions}
            optionLabel="description"
            optionValue="code"
            onChange={(e) => updateField('resultado_gestoria', e.value)}
            placeholder="Seleccione..."
            className="form-dropdown-presto w-full"
            disabled={isDisabled}
            itemTemplate={itemTemplate}
            valueTemplate={valueTemplate}
          />
        </div>

        <div className="flex flex-col gap-1.5 md:col-span-3">
          <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
            Observaciones
          </label>
          <InputTextarea
            value={formulario.observaciones ?? ''}
            onChange={(e) => updateField('observaciones', e.target.value)}
            rows={4}
            autoResize
            className="form-textarea-presto w-full"
            placeholder="Observaciones adicionales..."
            disabled={isDisabled}
          />
        </div>
      </div>
    </div>
  );
}
