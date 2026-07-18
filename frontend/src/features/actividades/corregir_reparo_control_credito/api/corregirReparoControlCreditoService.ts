import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoControlCredito } from '../models/corregirReparoControlCredito';

export const corregirReparoControlCreditoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoControlCredito | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoControlCredito | null>>(
      `/api/CorregirReparoControlCredito/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoControlCredito,
  ): Promise<ApiResponse<CorregirReparoControlCredito>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoControlCredito>>(
      `/api/CorregirReparoControlCredito/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoControlCredito/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
