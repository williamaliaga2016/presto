export interface administracion_rol {
  role_id: number;
  code?: string | null;
  name: string;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
