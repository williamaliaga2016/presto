import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CierreCopiasNotaria } from '../models/cierre_copias_notaria';

export const cierreCopiasNotariaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CierreCopiasNotaria | null>> {
    const response = await axiosClient.get<ApiResponse<CierreCopiasNotaria | null>>(
      `/api/CierreCopiasNotaria/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CierreCopiasNotaria,
  ): Promise<ApiResponse<CierreCopiasNotaria>> {
    const response = await axiosClient.post<ApiResponse<CierreCopiasNotaria>>(
      `/api/CierreCopiasNotaria/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CierreCopiasNotaria/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
