import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarControlCredito } from '../models/realizar_control_credito';

export const realizarControlCreditoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RealizarControlCredito | null>> {
    const response = await axiosClient.get<ApiResponse<RealizarControlCredito | null>>(
      `/api/RealizarControlCredito/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RealizarControlCredito,
  ): Promise<ApiResponse<RealizarControlCredito>> {
    const response = await axiosClient.post<ApiResponse<RealizarControlCredito>>(
      `/api/RealizarControlCredito/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RealizarControlCredito/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
