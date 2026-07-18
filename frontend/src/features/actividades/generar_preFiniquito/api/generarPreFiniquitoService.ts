import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarPreFiniquito } from '../models/generar_preFiniquito';

export const generarPreFiniquitoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GenerarPreFiniquito | null>> {
    const response = await axiosClient.get<ApiResponse<GenerarPreFiniquito | null>>(
      `/api/GenerarPreFiniquito/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },
  async save(
    payload: GenerarPreFiniquito,
  ): Promise<ApiResponse<GenerarPreFiniquito>> {
    const response = await axiosClient.post<ApiResponse<GenerarPreFiniquito>>(
      `/api/GenerarPreFiniquito/Save`,
      payload,
    );
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/GenerarPreFiniquito/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};