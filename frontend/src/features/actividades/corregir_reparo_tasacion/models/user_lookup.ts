export interface UserLookup {
  user_id: number;
  name?: string | null;
  last_name_first?: string | null;
  last_name_second?: string | null;
  user_name?: string | null;
  email?: string | null;
  is_active?: boolean | null;
  row_status?: boolean | null;

  name_complete?: string | null;
}
