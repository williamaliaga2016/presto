import { Calendar } from 'primereact/calendar';
import { InputMask } from 'primereact/inputmask';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';

import type { GestionarFirma } from '../models/gestionar_firma';

interface Props {
  form: GestionarFirma;
  isDisabled: boolean;
  updateField: <K extends keyof GestionarFirma>(
    field: K,
    value: GestionarFirma[K],
  ) => void;
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

export default function GestionarFirmaProgramacionSection({
  form,
  isDisabled,
  updateField,
}: Props) {
  return (
    <>
      <div className="flex flex-col gap-1.5">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Franja Horaria *</label>
        <InputMask
          value={form.franja_horaria ?? ''}
          onChange={(e) => updateField('franja_horaria', e.target.value ?? '')}
          mask="99:99"
          placeholder="--:--"
          className="form-input-presto w-full"
          disabled={isDisabled}
        />
      </div>

      <div className="flex flex-col gap-1.5">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Fecha Programacion Diligencia *</label>
        <Calendar
          value={toDateValue(form.fecha_programacion)}
          onChange={(e) =>
            updateField(
              'fecha_programacion',
              normalizeDate(e.value instanceof Date ? e.value.toISOString() : null),
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
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Ciudad Ubicada Cliente *</label>
        <InputText
          value={form.ciudad_cliente ?? ''}
          onChange={(e) => updateField('ciudad_cliente', e.target.value)}
          className="form-input-presto w-full"
          placeholder="Ej: Bogota"
          disabled={isDisabled}
        />
      </div>

      <div className="flex flex-col gap-1.5 md:col-span-3">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Direccion *</label>
        <InputText
          value={form.direccion_firma ?? ''}
          onChange={(e) => updateField('direccion_firma', e.target.value)}
          className="form-input-presto w-full"
          placeholder="Direccion completa..."
          disabled={isDisabled}
        />
      </div>

      <div className="flex flex-col gap-1.5 md:col-span-3">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Descripcion del Tramite *</label>
        <InputTextarea
          value={form.descripcion_tramite ?? ''}
          onChange={(e) => updateField('descripcion_tramite', e.target.value)}
          rows={4}
          autoResize
          className="form-textarea-presto w-full"
          placeholder="Detalles..."
          disabled={isDisabled}
        />
      </div>

      <div className="flex flex-col gap-1.5 md:col-span-3">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">Observaciones</label>
        <InputTextarea
          value={form.observaciones ?? ''}
          onChange={(e) => updateField('observaciones', e.target.value)}
          rows={4}
          autoResize
          className="form-textarea-presto w-full"
          placeholder="Ingrese las observaciones finales o aplicables para la actividad general..."
          disabled={isDisabled}
        />
      </div>
    </>
  );
}
