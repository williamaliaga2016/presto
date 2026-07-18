import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoPrefiniquito } from '../models/corregir_reparo_prefiniquito';

export const corregirReparoPrefiniquitoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoPrefiniquito | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoPrefiniquito | null>>(
      `/api/CorregirReparoPrefiniquito/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoPrefiniquito,
  ): Promise<ApiResponse<CorregirReparoPrefiniquito>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoPrefiniquito>>(
      `/api/CorregirReparoPrefiniquito/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoPrefiniquito/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
