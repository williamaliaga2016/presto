export interface LoginPayload {
  userId?: number;
  user_name: string;
  password: string;
  new_password?: string;
  confirm_password?: string;
  forgot_email?: string;
}
