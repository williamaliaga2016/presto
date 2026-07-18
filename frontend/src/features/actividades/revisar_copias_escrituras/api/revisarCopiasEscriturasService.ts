import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarCopiasEscrituras } from '../models/revisar_copias_escritura';
import { ControlesRevisarCopiasEscrituras } from '../models/catalogo';

export const revisarCopiasEscriturasService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarCopiasEscrituras | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarCopiasEscrituras | null>>(
      `/api/RevisarCopiasEscrituras/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RevisarCopiasEscrituras,
  ): Promise<ApiResponse<RevisarCopiasEscrituras>> {
    const response = await axiosClient.post<ApiResponse<RevisarCopiasEscrituras>>(
      `/api/RevisarCopiasEscrituras/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RevisarCopiasEscrituras/Avanzar/${id_expediente}`,
    );

    return response.data;
  },

  async getControlesRevisarCopiasEscrituras(): Promise<ApiResponse<ControlesRevisarCopiasEscrituras>> {
    const response = await axiosClient.get<ApiResponse<ControlesRevisarCopiasEscrituras>>(
      `/api/RevisarCopiasEscrituras/getControlesRevisarCopiasEscrituras`,
    );

    return response.data;
  },
};
