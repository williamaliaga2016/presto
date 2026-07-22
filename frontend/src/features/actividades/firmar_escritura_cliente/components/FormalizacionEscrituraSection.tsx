import InputTextForm from '@/shared/components/InputTextForm';
import InputCalendarForm from '@/shared/components/InputCalendarForm';
import DropdownForm from '@/shared/components/DropdownForm';
import type { FirmarEscrituraCliente } from '../models/firmar_escritura_cliente';
import type { CatalogoOption } from '@/models/CatalogoOption';

interface Props {
  form: FirmarEscrituraCliente;
  isDisabled: boolean;
  updateField: <K extends keyof FirmarEscrituraCliente>(
    field: K,
    value: FirmarEscrituraCliente[K],
  ) => void;
  representantesLegales: CatalogoOption[];
}

export default function FormalizacionEscrituraSection({
  form,
  isDisabled,
  updateField,
  representantesLegales,
}: Props) {
  const isRequired = form.requiere_escalamiento_comercial === 'NO';

  return (
    <>
      <InputTextForm
        label="Número Escritura"
        value={form.numero_escritura ?? ''}
        onChange={(val) => updateField('numero_escritura', val)}
        maxLength={20}
        placeholder="Número de escritura"
        disabled={isDisabled}
        required={isRequired}
      />

      <InputCalendarForm
        label="Fecha Escritura"
        value={form.fecha_escritura}
        onChange={(val) => updateField('fecha_escritura', val)}
        disabled={isDisabled}
        required={isRequired}
      />

      <DropdownForm
        label="Representante Legal"
        value={form.representante_legal}
        options={representantesLegales}
        onChange={(val) => updateField('representante_legal', val)}
        placeholder="Seleccionar..."
        disabled={isDisabled}
        filter
        filterPlaceholder="Buscar representante..."
      />
    </>
  );
}
