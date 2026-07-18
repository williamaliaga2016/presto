export interface AdministracionUsuario {
  user_id: number;
  role_id: number;
  name: string;
  last_name_first?: string | null;
  last_name_second?: string | null;
  id_document_type: number;
  nro_document?: string | null;
  user_name?: string | null;
  password?: string | null;
  email?: string | null;
  is_first_access?: boolean | null;
  remaining_attempts: number;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}