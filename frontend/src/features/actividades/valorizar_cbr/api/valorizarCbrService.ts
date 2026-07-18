import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValorizarCbr } from '../models/valorizar_cbr';

export const valorizarCbrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValorizarCbr | null>> {
    const response = await axiosClient.get<ApiResponse<ValorizarCbr | null>>(
      `/api/ValorizarCbr/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: ValorizarCbr,
  ): Promise<ApiResponse<ValorizarCbr>> {
    const response = await axiosClient.post<ApiResponse<ValorizarCbr>>(
      `/api/ValorizarCbr/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/ValorizarCbr/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
