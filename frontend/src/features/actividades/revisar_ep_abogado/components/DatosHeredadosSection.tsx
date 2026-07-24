import { formatDate } from '@/shared/utils/dateUtils';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';

interface Props {
  form: RevisarEpAbogado;
}

/**
 * Sección de datos heredados de la actividad "Firmar Escritura Cliente".
 * Solo labels de lectura — no son controles editables.
 */
export default function DatosHeredadosSection({ form }: Props) {
  const items = [
    { label: 'Notaría', value: form.notaria_desc },
    { label: 'Fecha Notaría', value: formatDate(form.fecha_notaria) },
    { label: 'Número Notaría', value: form.numero_notaria },
    { label: 'Ciudad Notaría', value: form.ciudad_notaria },
    { label: 'Número Escritura', value: form.numero_escritura },
    { label: 'Fecha Escritura', value: formatDate(form.fecha_escritura) },
  ];

  return (
    <>
      {items.map((item) => (
        <div key={item.label} className="field">
          <label className="block text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1">
            {item.label}
          </label>
          <span className="block text-sm text-gray-900">
            {item.value ?? '—'}
          </span>
        </div>
      ))}
    </>
  );
}
