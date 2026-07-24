import SwitchForm from '@/shared/components/SwitchForm';
import DropdownForm from '@/shared/components/DropdownForm';
import type { FirmarEscrituraCliente } from '../models/firmar_escritura_cliente';
import type { CatalogoOption } from '@/models/CatalogoOption';

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

  const conceptoEscalamiento = conceptosPrevios.find(
    (c) => c.area === 'ESCALAMIENTO_COMERCIAL',
  );
  const conceptoCausar = conceptosPrevios.find(
    (c) => c.area === 'LEASING_CAUSAR',
  );

  const handleEscalamientoChange = (value: boolean) => {
    const siNo = value ? 'SI' : 'NO';
    updateField('requiere_escalamiento_comercial', siNo);

    if (!value) {
      updateField('tipologia', null);
    }
  };

  const handleCausarChange = (value: boolean) => {
    updateField('requiere_causar', value ? 'SI' : 'NO');
  };

  return (
    <>
      {/* ¿Requiere Escalamiento Comercial? */}
      <SwitchForm
        label="¿Requiere Escalamiento Comercial?"
        value={form.requiere_escalamiento_comercial === 'SI'}
        onChange={handleEscalamientoChange}
        disabled={isDisabled || (conceptoEscalamiento?.existe ?? false)}
        required
        hint={
          conceptoEscalamiento?.existe
            ? 'Ya existe un dictamen previo para Escalamiento Comercial.'
            : undefined
        }
      />

      {/* Tipologías - visible solo si escalamiento = "SI" */}
      {form.requiere_escalamiento_comercial === 'SI' && (
        <DropdownForm
          label="Tipologías"
          value={form.tipologia}
          options={tipologias}
          onChange={(val) => updateField('tipologia', val)}
          placeholder="Seleccionar tipología..."
          disabled={isDisabled}
          required
        />
      )}

      {/* ¿Requiere Causar? - visible solo si tipo crédito es Leasing */}
      {isLeasing && (
        <SwitchForm
          label="¿Requiere Causar?"
          value={form.requiere_causar === 'SI'}
          onChange={handleCausarChange}
          disabled={isDisabled || (conceptoCausar?.existe ?? false)}
          required
          hint={
            conceptoCausar?.existe
              ? 'Ya existe un dictamen previo para Causación Leasing.'
              : undefined
          }
        />
      )}
    </>
  );
}
