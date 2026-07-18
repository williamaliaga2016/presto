import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoLiquidacion } from '../models/corregir_reparo_liquidacion';

export const corregirReparoLiquidacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoLiquidacion | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoLiquidacion | null>>(
      `/api/CorregirReparoLiquidacion/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoLiquidacion,
  ): Promise<ApiResponse<CorregirReparoLiquidacion>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoLiquidacion>>(
      `/api/CorregirReparoLiquidacion/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoLiquidacion/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
