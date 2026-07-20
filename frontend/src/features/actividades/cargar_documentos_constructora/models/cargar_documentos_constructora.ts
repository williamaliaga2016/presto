export interface CargarDocumentosConstructora {
  id: number;
  id_expediente: number;
  id_actividad: string;
  avanzar_validar_documentos: boolean;
  observaciones: string;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export const buildInitialState = (id_expediente: number): CargarDocumentosConstructora => ({
  id: 0,
  id_expediente,
  id_actividad: 'ACT_DOCS_CONSTRUCTORA',
  avanzar_validar_documentos: false,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: new Date().toISOString(),
  modified_by: null,
  modified_date: null,
});

export function validateAvanzarFields(
  form: Pick<CargarDocumentosConstructora, 'avanzar_validar_documentos' | 'observaciones'>,
): Set<string> {
  const missing = new Set<string>();

  if (!form.avanzar_validar_documentos) {
    missing.add('avanzar_validar_documentos');
  }

  if (!form.observaciones?.trim()) {
    missing.add('observaciones');
  }

  return missing;
}

export const normalizeCargarDocumentosConstructora = (
  source: Partial<CargarDocumentosConstructora> | null | undefined,
  id_expediente_fallback: number,
): CargarDocumentosConstructora => ({
  id: Number(source?.id ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback),
  id_actividad: source?.id_actividad ?? 'ACT_DOCS_CONSTRUCTORA',
  avanzar_validar_documentos: Boolean(source?.avanzar_validar_documentos ?? false),
  observaciones: source?.observaciones ?? '',
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: Number(source?.created_by ?? 0),
  created_date: source?.created_date ?? new Date().toISOString(),
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});
