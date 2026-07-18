import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { EntregarCarpeta } from '../models/entregar_carpeta';

export const entregarCarpetaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<EntregarCarpeta | null>> {
    const response = await axiosClient.get<ApiResponse<EntregarCarpeta | null>>(
      `/api/EntregarCarpeta/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: EntregarCarpeta,
  ): Promise<ApiResponse<EntregarCarpeta>> {
    const response = await axiosClient.post<ApiResponse<EntregarCarpeta>>(
      `/api/EntregarCarpeta/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/EntregarCarpeta/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
