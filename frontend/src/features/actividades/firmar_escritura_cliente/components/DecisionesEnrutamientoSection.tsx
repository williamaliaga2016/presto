import { Dropdown } from 'primereact/dropdown';
import { SelectButton } from 'primereact/selectbutton';

import type { FirmarEscrituraCliente } from '../models/firmar_escritura_cliente';
import type { CatalogoOption } from '@/models/CatalogoOption';

const SI_NO_OPTIONS = [
  { label: 'SÍ', value: 'SI' },
  { label: 'NO', value: 'NO' },
];

interface ConceptoPrevio {
  area: string;
  existe: boolean;
}

interface Props {
  form: FirmarEscrituraCliente;
  isDisabled: boolean;
  updateField: <K extends keyof FirmarEscrituraCliente>(
    field: K,
    value: FirmarEscrituraCliente[K],
  ) => void;
  tipologias: CatalogoOption[];
  tiposLeasing: CatalogoOption[];
  conceptosPrevios?: ConceptoPrevio[];
}

export default function DecisionesEnrutamientoSection({
  form,
  isDisabled,
  updateField,
  tipologias,
  tiposLeasing,
  conceptosPrevios = [],
}: Props) {
  const isLeasing = tiposLeasing.some(t => t.code === form.tipo_credito);

  const handleEscalamientoChange = (value: 'SI' | 'NO' | null) => {
    updateField('requiere_escalamiento_comercial', value);

    // Limpiar tipología cuando se cambia a "NO" o null
    if (value !== 'SI') {
      updateField('tipologia', null);
    }
  };

  const conceptoEscalamiento = conceptosPrevios.find(
    (c) => c.area === 'ESCALAMIENTO_COMERCIAL',
  );
  const conceptoCausar = conceptosPrevios.find(
    (c) => c.area === 'LEASING_CAUSAR',
  );

  return (
    <>
      {/* ¿Requiere Escalamiento Comercial? */}
      <div className="flex flex-col gap-1.5">
        <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
          ¿Requiere Escalamiento Comercial? *
        </label>
        <SelectButton
          value={form.requiere_escalamiento_comercial}
          options={SI_NO_OPTIONS}
          onChange={(e) => handleEscalamientoChange(e.value)}
          disabled={isDisabled || (conceptoEscalamiento?.existe ?? false)}
        />
        {conceptoEscalamiento?.existe && (
          <span className="text-xs text-orange-600">
            Ya existe un dictamen previo para Escalamiento Comercial.
          </span>
        )}
      </div>

      {/* Tipologías - visible solo si escalamiento = "SI" */}
      {form.requiere_escalamiento_comercial === 'SI' && (
        <div className="flex flex-col gap-1.5">
          <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
            Tipologías *
          </label>
          <Dropdown
            value={form.tipologia}
            options={tipologias}
            optionLabel="description"
            optionValue="code"
            onChange={(e) => updateField('tipologia', e.value)}
            placeholder="Seleccionar tipología..."
            className="w-full"
            disabled={isDisabled}
          />
        </div>
      )}

      {/* ¿Requiere Causar? - visible solo si tipo crédito es Leasing */}
      {isLeasing && (
        <div className="flex flex-col gap-1.5">
          <label className="text-xs font-semibold uppercase tracking-wide text-slate-700">
            ¿Requiere Causar? *
          </label>
          <SelectButton
            value={form.requiere_causar}
            options={SI_NO_OPTIONS}
            onChange={(e) => updateField('requiere_causar', e.value)}
            disabled={isDisabled || (conceptoCausar?.existe ?? false)}
          />
          {conceptoCausar?.existe && (
            <span className="text-xs text-orange-600">
              Ya existe un dictamen previo para Causación Leasing.
            </span>
          )}
        </div>
      )}
    </>
  );
}
