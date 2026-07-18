import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecepcionCargaFabrica } from '../models/recepcion_carga_fabrica';

export const recepcionCargaFabricaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RecepcionCargaFabrica | null>> {
    const response = await axiosClient.get<ApiResponse<RecepcionCargaFabrica | null>>(
      `/api/RecepcionCargaFabrica/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RecepcionCargaFabrica,
  ): Promise<ApiResponse<RecepcionCargaFabrica>> {
    const response = await axiosClient.post<ApiResponse<RecepcionCargaFabrica>>(
      `/api/RecepcionCargaFabrica/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RecepcionCargaFabrica/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
