import type { DocumentosObligatoriosRDI } from '../models/revisar_documentos_inmueble';

type Props = {
  data: DocumentosObligatoriosRDI;
};

/**
 * Alerta informativa (CA8): no bloquea el guardado, solo advierte cuando
 * falta alguno de los 3 documentos mínimos obligatorios (Promesa de
 * Compraventa, Escritura, CTL) en el Expediente Digital. Sí es bloqueante
 * al momento de /avanzar con documentos_correctos = true (lo valida el
 * backend, ver sección 2.4 de la especificación).
 */
export default function DocumentosObligatoriosAlert({ data }: Props) {
  if (data.completos) return null;

  return (
    <div className="mb-4 rounded-md border border-yellow-200 bg-yellow-50 px-4 py-3 text-sm text-yellow-800">
      <strong>Documentos obligatorios incompletos:</strong> faltan{' '}
      {data.faltantes.length > 0 ? data.faltantes.join(', ') : 'documentos mínimos'} en el
      Expediente Digital. No podrá marcar ¿Documentos Correctos? = Sí ni avanzar la actividad
      como correcta mientras falte alguno.
    </div>
  );
}