import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { HistorialExpediente } from '../models/historialExpediente';

export const historialExpedienteService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<HistorialExpediente[]>> {
    const response = await axiosClient.get<ApiResponse<HistorialExpediente[]>>(
      `/api/HistorialExpediente/GetByIdExpediente/${id_expediente}`,
    );

    return response.data;
  },
};
