import { axiosClient } from '@/core/api/axiosClient';

import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AdministracionUsuario } from '../models/AdministracionUsuariosDTO';

import { API_BASE_URL } from '../../../../config';

const BASE_URL = `${API_BASE_URL}/api/User`;

export const administracionUsuarioService = {
  async getUsers(): Promise<ApiResponse<AdministracionUsuario[]>> {
    const response = await axiosClient.get<
      ApiResponse<AdministracionUsuario[]>
    >(`${BASE_URL}/GetUsers`);

    return response.data;
  },

  async create(
    payload: AdministracionUsuario,
  ): Promise<ApiResponse<AdministracionUsuario>> {
    const response = await axiosClient.post<ApiResponse<AdministracionUsuario>>(
      `${BASE_URL}/Create`,
      payload,
    );

    return response.data;
  },

  async update(
    payload: AdministracionUsuario,
  ): Promise<ApiResponse<AdministracionUsuario>> {
    const response = await axiosClient.put<ApiResponse<AdministracionUsuario>>(
      `${BASE_URL}/Update`,
      payload,
    );

    return response.data;
  },

  async delete(userId: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.delete<ApiResponse<boolean>>(
      `${BASE_URL}/Delete/${userId}`,
    );

    return response.data;
  },
};
