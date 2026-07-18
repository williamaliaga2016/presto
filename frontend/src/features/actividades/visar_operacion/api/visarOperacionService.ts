import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VisarOperacion } from '../models/visar_operacion';

export const visarOperacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<VisarOperacion | null>> {
    const response = await axiosClient.get<ApiResponse<VisarOperacion | null>>(
      `/api/VisarOperacion/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: VisarOperacion,
  ): Promise<ApiResponse<VisarOperacion>> {
    const response = await axiosClient.post<ApiResponse<VisarOperacion>>(
      `/api/VisarOperacion/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/VisarOperacion/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
