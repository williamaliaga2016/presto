import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { Bitacora } from '../models/Bitacora';

export const bitacoraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<Bitacora[]>> {
    const response = await axiosClient.get<ApiResponse<Bitacora[]>>(
      `/api/Bitacora/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async create(payload: Bitacora): Promise<ApiResponse<Bitacora>> {
    const response = await axiosClient.post<ApiResponse<Bitacora>>(
      '/api/Bitacora/Create',
      payload,
    );
    return response.data;
  },
};
