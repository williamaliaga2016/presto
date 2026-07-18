import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarLiquidacion } from '../models/revisar_liquidacion';

export const revisarLiquidacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarLiquidacion | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarLiquidacion | null>>(
      `/api/RevisarLiquidacion/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },
  async save(
    payload: RevisarLiquidacion,
  ): Promise<ApiResponse<RevisarLiquidacion>> {
    const response = await axiosClient.post<ApiResponse<RevisarLiquidacion>>(
      `/api/RevisarLiquidacion/Save`,
      payload,
    );
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RevisarLiquidacion/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
