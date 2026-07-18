export interface RectificatoriaLegalFirmaAlzante {
  id_rectificatoria_legal_firma_alzante: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  fecha_firma_alzante: string | null;
  observaciones: string | null;
  nombre_banco_alzante: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
