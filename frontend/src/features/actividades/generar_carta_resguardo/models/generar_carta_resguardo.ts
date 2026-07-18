export interface GenerarCartaResguardo {
  id_generar_carta_resguardo: number;
  id_expediente: number;
  generar_carta: boolean;
  tipo_carta?: string | null;
  enviar_a_reparo: boolean;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
