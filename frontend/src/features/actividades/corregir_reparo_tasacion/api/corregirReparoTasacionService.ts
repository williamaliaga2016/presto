import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoTasacion } from '../models/corregir_reparo_tasacion';

export const corregirReparoTasacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoTasacion | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoTasacion | null>>(
      `/api/CorregirReparoTasacion/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoTasacion,
  ): Promise<ApiResponse<CorregirReparoTasacion>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoTasacion>>(
      `/api/CorregirReparoTasacion/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoTasacion/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
