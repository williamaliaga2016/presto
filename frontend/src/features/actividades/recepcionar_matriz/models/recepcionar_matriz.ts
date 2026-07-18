export interface RecepcionarMatriz {
  id_recepcionar_matriz: number;
  id_expediente: number;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
