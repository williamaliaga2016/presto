import InputTextForm from '@/shared/components/InputTextForm';
import InputNumberForm from '@/shared/components/InputNumberForm';
import InputCalendarForm from '@/shared/components/InputCalendarForm';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';

interface Props {
  form: RevisarEpAbogado;
}

/**
 * Sección de datos heredados de la actividad "Firmar Escritura Cliente".
 * Todos los campos se renderizan en modo estricto de solo lectura (disabled).
 * Corresponde al requerimiento CA02: Herencia de datos en solo lectura.
 */
export default function DatosHeredadosSection({ form }: Props) {
  const noop = () => {};

  return (
    <>
      <InputTextForm
        label="Notaría"
        value={form.notaria ?? ''}
        onChange={noop}
        disabled
      />

      <InputCalendarForm
        label="Fecha Notaría"
        value={form.fecha_notaria}
        onChange={noop}
        disabled
      />

      <InputNumberForm
        label="Número Notaría"
        value={form.numero_notaria}
        onChange={noop}
        disabled
      />

      <InputTextForm
        label="Ciudad Notaría"
        value={form.ciudad_notaria ?? ''}
        onChange={noop}
        disabled
      />

      <InputTextForm
        label="Número Escritura"
        value={form.numero_escritura ?? ''}
        onChange={noop}
        disabled
      />

      <InputCalendarForm
        label="Fecha Escritura"
        value={form.fecha_escritura}
        onChange={noop}
        disabled
      />
    </>
  );
}
