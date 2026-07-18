export interface RevisarInscripcionCbr {
  id_revisar_inscripcion_cbr: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_enviar_reparo: boolean;
  observaciones?: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
