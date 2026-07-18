import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ReingresarEscrituraCbr } from '../models/reingresar_escritura_cbr';

export const reingresarEscrituraCbrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ReingresarEscrituraCbr | null>> {
    const response = await axiosClient.get<ApiResponse<ReingresarEscrituraCbr | null>>(
      `/api/ReingresarEscrituraCbr/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: ReingresarEscrituraCbr,
  ): Promise<ApiResponse<ReingresarEscrituraCbr>> {
    const response = await axiosClient.post<ApiResponse<ReingresarEscrituraCbr>>(
      `/api/ReingresarEscrituraCbr/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/ReingresarEscrituraCbr/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
