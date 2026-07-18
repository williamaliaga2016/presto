import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaSolucionReparo } from '../models/gestion_rectificatoria_solucion_reparo';

const baseUrl = '/api/GestionRectificatoriaSolucionReparo';

export const GestionRectificatoriaSolucionReparoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GestionRectificatoriaSolucionReparo | null>> {
    const response = await axiosClient.get<ApiResponse<GestionRectificatoriaSolucionReparo | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: GestionRectificatoriaSolucionReparo,
  ): Promise<ApiResponse<GestionRectificatoriaSolucionReparo>> {
    const response = await axiosClient.post<ApiResponse<GestionRectificatoriaSolucionReparo>>(
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