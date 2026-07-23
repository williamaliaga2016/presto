import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import type { ControlesFirmarRepLegal } from '../models/controles';

const PATH_URL = '/api/firmar-rep-legal';

export const firmarRepLegalService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<FirmarRepLegal | null>> {
    const response = await axiosClient.get<ApiResponse<FirmarRepLegal | null>>(
      `${PATH_URL}/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(
    payload: FirmarRepLegal,
  ): Promise<ApiResponse<FirmarRepLegal>> {
    const response = await axiosClient.post<ApiResponse<FirmarRepLegal>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `${PATH_URL}/avanzar/${id_expediente}`,
    );
    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesFirmarRepLegal>> {
    const response = await axiosClient.get<ApiResponse<ControlesFirmarRepLegal>>(
      `${PATH_URL}/controles`,
    );
    return response.data;
  },
};
