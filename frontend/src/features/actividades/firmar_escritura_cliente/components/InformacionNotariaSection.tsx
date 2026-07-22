import InputTextForm from '@/shared/components/InputTextForm';
import InputNumberForm from '@/shared/components/InputNumberForm';
import InputCalendarForm from '@/shared/components/InputCalendarForm';
import type { FirmarEscrituraCliente } from '../models/firmar_escritura_cliente';

interface Props {
  form: FirmarEscrituraCliente;
  isDisabled: boolean;
  updateField: <K extends keyof FirmarEscrituraCliente>(
    field: K,
    value: FirmarEscrituraCliente[K],
  ) => void;
}

export default function InformacionNotariaSection({
  form,
  isDisabled,
  updateField,
}: Props) {
  return (
    <>
      <InputTextForm
        label="Notaría"
        value={form.notaria ?? ''}
        onChange={(val) => updateField('notaria', val)}
        maxLength={150}
        placeholder="Nombre de la notaría"
        disabled={isDisabled}
        required
      />

      <InputCalendarForm
        label="Fecha Notaría"
        value={form.fecha_notaria}
        onChange={(val) => updateField('fecha_notaria', val)}
        disabled={isDisabled}
        required
      />

      <InputNumberForm
        label="Número Notaría"
        value={form.numero_notaria}
        onChange={(val) => updateField('numero_notaria', val)}
        min={1}
        max={9999}
        placeholder="Ej: 1234"
        disabled={isDisabled}
        required
      />

      <InputTextForm
        label="Ciudad Notaría"
        value={form.ciudad_notaria ?? ''}
        onChange={(val) => updateField('ciudad_notaria', val)}
        maxLength={100}
        placeholder="Ej: Bogotá"
        disabled={isDisabled}
        required
      />
    </>
  );
}
