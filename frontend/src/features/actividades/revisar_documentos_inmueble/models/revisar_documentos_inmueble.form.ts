import type { RevisarDocumentosInmueble } from './revisar_documentos_inmueble';

export const buildInitialState = (
  id_expediente: number,
): RevisarDocumentosInmueble => ({
  id: 0,
  id_expediente,
  id_actividad: 'ACT_REVISAR_DOCS',
  documentos_correctos: null,
  motivo_devolucion: null,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

export function validateAvanzarFields(
  form: Pick<
    RevisarDocumentosInmueble,
    'documentos_correctos' | 'motivo_devolucion' | 'observaciones'
  >,
): Set<string> {
  const missing = new Set<string>();

  if (
    form.documentos_correctos === null ||
    form.documentos_correctos === undefined
  ) {
    missing.add('documentos_correctos');
  }

  if (
    form.documentos_correctos === false &&
    !form.motivo_devolucion?.trim()
  ) {
    missing.add('motivo_devolucion');
  }

  if (!form.observaciones?.trim()) {
    missing.add('observaciones');
  }

  return missing;
}

export const normalizeRevisarDocumentosInmueble = (
  source: Partial<RevisarDocumentosInmueble> | null | undefined,
  id_expediente_fallback: number,
): RevisarDocumentosInmueble => ({
  id: Number(source?.id ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  id_actividad: source?.id_actividad ?? 'ACT_REVISAR_DOCS',
  documentos_correctos: source?.documentos_correctos ?? null,
  motivo_devolucion: source?.motivo_devolucion ?? null,
  observaciones: source?.observaciones ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? new Date().toISOString(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});
