import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { LoginPayload } from '../models/LoginPayload';
import type { AuthUser } from '../models/AuthUser';

export const authService = {
  async login(payload: LoginPayload): Promise<ApiResponse<AuthUser>> {
    const response = await axiosClient.post<ApiResponse<AuthUser>>(
      '/api/security/Login/auth',
      payload,
    );
    return response.data;
  },
};
