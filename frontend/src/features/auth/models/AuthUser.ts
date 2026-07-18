export interface AuthUser {
  user_id?: number;
  role_id?: number;
  user_name?: string;
  role_name?: string;
  name_complete?: string;
  email?: string;
  token_multibanca?: string;
  token_captcha?: string;
  status?: boolean;
  message?: string;
}
