import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoEntregarCarpeta } from '../models/corregir_reparo_entregar_carpeta';

export const corregirReparoEntregarCarpetaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoEntregarCarpeta | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoEntregarCarpeta | null>>(
      `/api/CorregirReparoEntregarCarpeta/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoEntregarCarpeta,
  ): Promise<ApiResponse<CorregirReparoEntregarCarpeta>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoEntregarCarpeta>>(
      `/api/CorregirReparoEntregarCarpeta/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoEntregarCarpeta/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
