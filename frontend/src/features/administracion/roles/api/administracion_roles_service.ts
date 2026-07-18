import { axiosClient } from '@/core/api/axiosClient';

import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { administracion_rol } from '../models/administracion_roles_dto';

import { API_BASE_URL } from '../../../../config';

const BASE_URL = `${API_BASE_URL}/api/Role`;

export const administracion_roles_service = {
  async get_roles(): Promise<ApiResponse<administracion_rol[]>> {
    const response = await axiosClient.get<ApiResponse<administracion_rol[]>>(
      `${BASE_URL}/GetRoles`,
    );

    return response.data;
  },

  async create(
    payload: administracion_rol,
  ): Promise<ApiResponse<administracion_rol>> {
    const response = await axiosClient.post<ApiResponse<administracion_rol>>(
      `${BASE_URL}/Create`,
      payload,
    );

    return response.data;
  },

  async update(
    payload: administracion_rol,
  ): Promise<ApiResponse<administracion_rol>> {
    const response = await axiosClient.put<ApiResponse<administracion_rol>>(
      `${BASE_URL}/Update`,
      payload,
    );

    return response.data;
  },

  async delete(role_id: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.delete<ApiResponse<boolean>>(
      `${BASE_URL}/Delete/${role_id}`,
    );

    return response.data;
  },
};
