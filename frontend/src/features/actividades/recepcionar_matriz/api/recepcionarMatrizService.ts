import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecepcionarMatriz } from '../models/recepcionar_matriz';

export const recepcionarMatrizService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RecepcionarMatriz | null>> {
    const response = await axiosClient.get<ApiResponse<RecepcionarMatriz | null>>(
      `/api/RecepcionarMatriz/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RecepcionarMatriz,
  ): Promise<ApiResponse<RecepcionarMatriz>> {
    const response = await axiosClient.post<ApiResponse<RecepcionarMatriz>>(
      `/api/RecepcionarMatriz/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RecepcionarMatriz/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
