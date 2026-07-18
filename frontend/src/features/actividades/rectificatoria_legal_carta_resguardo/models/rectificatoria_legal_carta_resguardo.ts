export interface RectificatoriaLegalCartaResguardo {
  id_rectificatoria_legal_carta_resguardo: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  observaciones?: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
