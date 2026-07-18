
import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoria } from '../models/gestion_rectificatoria';
import type { ControlesGestionRectificatoria } from '../models/catalogo';

const baseUrl = '/api/GestionRectificatoria';

export const GestionRectificatoriaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GestionRectificatoria | null>> {
    const response = await axiosClient.get<ApiResponse<GestionRectificatoria | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: GestionRectificatoria,
  ): Promise<ApiResponse<GestionRectificatoria>> {
    const response = await axiosClient.post<ApiResponse<GestionRectificatoria>>(
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
    async getControles(): Promise<ApiResponse<ControlesGestionRectificatoria>> {
      const response = await axiosClient.get<ApiResponse<ControlesGestionRectificatoria>>(
        `${baseUrl}/GetControles`,
      );
  
      return response.data;
    },
};

