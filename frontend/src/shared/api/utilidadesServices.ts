import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '../models/ControlBaseDTO';
import type { UtilidadesDTO } from '../models/UtilidadesDTO';

export const utilidadesServices = {
  async catalogTypeUtility(): Promise<ApiResponse<ControlBaseDTO[]>> {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      `/api/Utilidades/CatalogTypeUtility`,
    );

    return response.data;
  },

  async getActivities(
    payload: UtilidadesDTO,
  ): Promise<ApiResponse<ControlBaseDTO[]>> {
    const response = await axiosClient.post<ApiResponse<ControlBaseDTO[]>>(
      `/api/Utilidades/GetActivities`,
      payload,
    );

    return response.data;
  },

  async validateRequestNumber(
    id_expediente: string,
  ): Promise<ApiResponse<number>> {
    const response = await axiosClient.get<ApiResponse<number>>(
      `/api/Utilidades/ValidateRequestNumber/${id_expediente}`,
    );

    return response.data;
  },

  async getUserActivity(
    payload: UtilidadesDTO,
  ): Promise<ApiResponse<ControlBaseDTO[]>> {
    const response = await axiosClient.post<ApiResponse<ControlBaseDTO[]>>(
      `/api/Utilidades/GetUserActivity`,
      payload,
    );

    return response.data;
  },

  async save(payload: UtilidadesDTO): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `/api/Utilidades/save`,
      payload,
    );

    return response.data;
  },
};
