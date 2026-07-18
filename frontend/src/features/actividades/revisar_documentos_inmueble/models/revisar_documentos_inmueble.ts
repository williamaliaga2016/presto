export interface RevisarDocumentosInmueble {
  id: number;
  id_expediente: number;
  id_actividad: string;
  documentos_correctos: boolean | null;
  motivo_devolucion: string | null;
  observaciones: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
