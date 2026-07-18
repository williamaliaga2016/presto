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
