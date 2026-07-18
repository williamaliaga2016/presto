import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ActividadDTO } from '../models/ActividadDTO';

export const actividadDashboardService = {
  async getInfoActivityByUser(): Promise<ApiResponse<ActividadDTO[]>> {
    const response = await axiosClient.get<ApiResponse<ActividadDTO[]>>(
      '/api/BandejaActividades/getInfoActivityByUser',
    );

    return response.data;
  },

  async updateStatus(
    actividad: ActividadDTO,
  ): Promise<ApiResponse<ActividadDTO>> {
    const response = await axiosClient.post<ApiResponse<ActividadDTO>>(
      '/api/BandejaActividades/updateStatus',
      actividad,
    );

    return response.data;
  },
};
