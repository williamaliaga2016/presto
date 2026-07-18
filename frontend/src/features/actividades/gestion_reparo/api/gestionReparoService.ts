import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionReparo } from '../models/gestion_reparo';

const baseUrl = '/api/GestionReparo';

export const GestionReparoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GestionReparo | null>> {
    const response = await axiosClient.get<ApiResponse<GestionReparo | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: GestionReparo,
  ): Promise<ApiResponse<GestionReparo>> {
    const response = await axiosClient.post<ApiResponse<GestionReparo>>(
      `${baseUrl}/Save`,
      payload
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `${baseUrl}/Avanzar/${id_expediente}`
    );
    return response.data;
  },
};