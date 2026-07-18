import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarCartaResguardo } from '../models/generar_carta_resguardo';
import { ControlesGenerarCartaResguardo } from '../models/catalogo';

export const generarCartaResguardoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GenerarCartaResguardo | null>> {
    const response = await axiosClient.get<ApiResponse<GenerarCartaResguardo | null>>(
      `/api/GenerarCartaResguardo/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: GenerarCartaResguardo,
  ): Promise<ApiResponse<GenerarCartaResguardo>> {
    const response = await axiosClient.post<ApiResponse<GenerarCartaResguardo>>(
      `/api/GenerarCartaResguardo/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/GenerarCartaResguardo/Avanzar/${id_expediente}`,
    );

    return response.data;
  },

  async getControlesGenerarCartaResguardo(): Promise<ApiResponse<ControlesGenerarCartaResguardo>> {
    const response = await axiosClient.get<ApiResponse<ControlesGenerarCartaResguardo>>(
      `/api/GenerarCartaResguardo/getControlesGenerarCartaResguardo`,
    );

    return response.data;
  },
};
