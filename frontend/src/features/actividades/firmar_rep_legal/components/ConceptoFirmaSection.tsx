import DropdownForm from '@/shared/components/DropdownForm';
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
      {/* Concepto de Firma — Dropdown L41 */}
      <DropdownForm
        label="Concepto de Firma"
        value={form.concepto_firma}
        options={conceptoOptions}
        onChange={(val) => onConceptoChange(val ?? '')}
        placeholder="Seleccione concepto..."
        disabled={isDisabled}
        required
      />

      {/* Campos condicionales: solo visibles si "Escritura NO firmada" (CRL-2) */}
      {form.concepto_firma === 'CRL-2' && (
        <>
          <DropdownForm
            label="Tipología"
            value={form.tipologia}
            options={tipologiaOptions}
            onChange={(val) => onTipologiaChange(val ?? '')}
            placeholder="Seleccionar tipología..."
            disabled={isDisabled}
            required
          />

          <DropdownForm
            label="Casuística"
            value={form.casuistica}
            options={casuisticaOptions}
            onChange={(val) => updateField('casuistica', val)}
            placeholder={
              !form.tipologia
                ? 'Seleccione primero una tipología'
                : casuisticaOptions.length === 0
                  ? 'Sin casuísticas configuradas'
                  : 'Seleccionar casuística...'
            }
            disabled={isDisabled || !form.tipologia}
            required
          />

          <div className="md:col-span-3">
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
