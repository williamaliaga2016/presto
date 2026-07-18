import axios from 'axios';
import { API_BASE_URL } from '../../../config';
import type { LoginResponse } from '../../../models/LoginResponse';

export const loginService = async (
  user_name: string,
  password: string,
): Promise<LoginResponse> => {
  try {
    const response = await axios.post<LoginResponse>(
      `${API_BASE_URL}/api/security/Login/auth`,
      {
        user_name,
        password,
      },
    );

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message || 'Error al iniciar sesión';
  }
};
