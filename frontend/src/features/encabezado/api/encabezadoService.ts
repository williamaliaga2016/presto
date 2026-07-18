import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { EncabezadoDTO } from '../models/encabezado';

export const encabezadoService = {
  async getInfoEncabezado(
    idExpediente: number,
    activityID?: string,
  ): Promise<ApiResponse<EncabezadoDTO | null>> {
    const response = await axiosClient.get<ApiResponse<EncabezadoDTO | null>>(
      `/api/Encabezado/infoEncabezado/${idExpediente}`,
      {
        params: activityID?.trim() ? { activityID } : undefined,
      },
    );

    return response.data;
  },
};
