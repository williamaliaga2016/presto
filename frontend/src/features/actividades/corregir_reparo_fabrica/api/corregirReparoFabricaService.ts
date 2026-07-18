import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoFabrica } from '../models/corregir_reparo_fabrica';

export const corregirReparoFabricaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoFabrica | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoFabrica | null>>(
      `/api/CorregirReparoFabrica/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoFabrica,
  ): Promise<ApiResponse<CorregirReparoFabrica>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoFabrica>>(
      `/api/CorregirReparoFabrica/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoFabrica/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
