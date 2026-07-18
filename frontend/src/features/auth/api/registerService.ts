import axios from 'axios';
import { API_BASE_URL } from '../../../config';
import type { RegisterResponse } from '../../../models/RegisterResponse';

export const registerService = async (
  user_name: string,
  password: string,
  confirmPassword: string,
): Promise<RegisterResponse> => {
  if (password !== confirmPassword) {
    throw new Error('Las contraseñas no coinciden');
  }

  try {
    const response = await axios.post<RegisterResponse>(
      `${API_BASE_URL}/api/User/Create`,
      {
        user_name,
        password,
      },
    );

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message || 'Error al registrar usuario';
  }
};
