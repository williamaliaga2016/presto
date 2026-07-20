import { Checkbox, CheckboxChangeEvent } from 'primereact/checkbox';
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

export default function GestionarFirmaElectronicaSection({
  form,
  isDisabled,
  updateField,
}: Props) {
  const handleChange = (e: CheckboxChangeEvent) => {
    updateField('firma_electronica_realizada', Boolean(e.checked));
  };

  return (
    <>
      <div className="flex flex-col gap-1.5 md:col-span-3">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">&nbsp;</label>
        <div className="flex items-center gap-2 h-10">
          <Checkbox
            className="form-checkbox-presto"
            inputId="firma_electronica_realizada"
            checked={Boolean(form.firma_electronica_realizada)}
            onChange={handleChange}
            disabled={isDisabled}
          />
          <label htmlFor="firma_electronica_realizada" className="text-sm font-medium text-slate-700">
            Firma Electronica Realizada *
          </label>
        </div>
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
