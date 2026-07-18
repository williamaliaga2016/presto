import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparosGestor } from '../models/corregir_reparos_gestor';

export const corregirReparosGestorService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparosGestor | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparosGestor | null>>(
      `/api/CorregirReparosGestor/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparosGestor,
  ): Promise<ApiResponse<CorregirReparosGestor>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparosGestor>>(
      `/api/CorregirReparosGestor/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparosGestor/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
