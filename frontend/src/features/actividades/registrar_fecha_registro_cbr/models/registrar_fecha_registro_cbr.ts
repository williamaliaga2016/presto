export interface RegistrarFechaRegistroCbr {
  id_registrar_fecha_registro_cbr: number;
  id_expediente: number;
  fecha_registro_cbr: string | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
