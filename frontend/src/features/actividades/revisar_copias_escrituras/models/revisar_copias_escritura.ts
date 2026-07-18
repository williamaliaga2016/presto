export interface RevisarCopiasEscrituras {
  id_revisar_copias_escrituras: number;
  id_expediente: number;
  cbr?: string | null;
  enviar_a_reparo: boolean;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
