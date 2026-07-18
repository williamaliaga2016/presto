export interface GestionRectificatoria {
  id_gestion_rectificatoria: number;
  id_expediente: number;
  enviar_tipo_reparo: string;
  observaciones?: string | null;
  
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}