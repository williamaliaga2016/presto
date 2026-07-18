export interface GenerarEstudioTitulos {
  id_generar_estudio_titulos: number;
  id_expediente: number;
  observaciones?: string | null;
  enviar_reparo: boolean;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}