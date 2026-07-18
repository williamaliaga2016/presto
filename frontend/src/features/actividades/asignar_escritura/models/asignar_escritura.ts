export interface AsignarEscritura {
  id_asignar_escritura: number;
  id_expediente: number;
  id_actividad: string;
  abogado: string;
  observaciones: string;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
