import { Dropdown } from 'primereact/dropdown';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

interface Props {
  form: FirmarRepLegal;
  isDisabled: boolean;
  updateField: <K extends keyof FirmarRepLegal>(
    field: K,
    value: FirmarRepLegal[K],
  ) => void;
  conceptoOptions: ControlBaseDTO[];
  tipologiaOptions: ControlBaseDTO[];
  casuisticaOptions: ControlBaseDTO[];
  onConceptoChange: (value: string) => void;
  onTipologiaChange: (value: string) => void;
}

export default function ConceptoFirmaSection({
  form,
  isDisabled,
  updateField,
  conceptoOptions,
  tipologiaOptions,
  casuisticaOptions,
  onConceptoChange,
  onTipologiaChange,
}: Props) {
  return (
    <>
      {/* Concepto de Firma (L41) */}
      <div className="flex flex-col gap-1.5">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
          Concepto de Firma *
        </label>
        <Dropdown
          value={form.concepto_firma}
          options={conceptoOptions}
          optionLabel="description"
          optionValue="code"
          onChange={(e) => onConceptoChange(e.value)}
          placeholder="Seleccionar concepto..."
          className="w-full"
          disabled={isDisabled}
        />
      </div>

      {/* Campos condicionales: solo visibles si "Escritura NO firmada" (CRL-2) */}
      {form.concepto_firma === 'CRL-2' && (
        <>
          {/* Tipología (L42) */}
          <div className="flex flex-col gap-1.5">
            <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
              Tipología *
            </label>
            <Dropdown
              value={form.tipologia}
              options={tipologiaOptions}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => onTipologiaChange(e.value)}
              placeholder="Seleccionar tipología..."
              className="w-full"
              disabled={isDisabled}
            />
          </div>

          {/* Casuística (L43) — dependiente de tipología */}
          <div className="flex flex-col gap-1.5">
            <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
              Casuística *
            </label>
            <Dropdown
              value={form.casuistica}
              options={casuisticaOptions}
              optionLabel="description"
              optionValue="code"
              onChange={(e) => updateField('casuistica', e.value)}
              placeholder={
                !form.tipologia
                  ? 'Seleccione primero una tipología'
                  : casuisticaOptions.length === 0
                    ? 'Sin casuísticas configuradas'
                    : 'Seleccionar casuística...'
              }
              className="w-full"
              disabled={isDisabled || !form.tipologia}
            />
          </div>

          {/* Observaciones */}
          <div className="md:col-span-3 flex flex-col gap-1.5">
            <InputTextAreaForm
              label="Observaciones"
              value={form.observaciones ?? ''}
              onChange={(val) => updateField('observaciones', val || null)}
              maxLength={500}
              rows={4}
              placeholder="Indique el motivo de la no firma (máximo 500 caracteres)"
              disabled={isDisabled}
              required
            />
          </div>
        </>
      )}
    </>
  );
}
