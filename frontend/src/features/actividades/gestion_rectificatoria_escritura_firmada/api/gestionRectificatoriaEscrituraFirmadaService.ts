import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaEscrituraFirmada } from '../models/gestion_rectificatoria_escritura_firmada';
import type { ControlesGestionRectificatoriaEscrituraFirmada } from '../models/catalogo';

const baseUrl = '/api/GestionRectificatoriaEscrituraFirmada';

export const GestionRectificatoriaEscrituraFirmadaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GestionRectificatoriaEscrituraFirmada | null>> {
    const response = await axiosClient.get<ApiResponse<GestionRectificatoriaEscrituraFirmada | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: GestionRectificatoriaEscrituraFirmada,
  ): Promise<ApiResponse<GestionRectificatoriaEscrituraFirmada>> {
    const response = await axiosClient.post<ApiResponse<GestionRectificatoriaEscrituraFirmada>>(
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

  async getControles(): Promise<ApiResponse<ControlesGestionRectificatoriaEscrituraFirmada>> {
        const response = await axiosClient.get<ApiResponse<ControlesGestionRectificatoriaEscrituraFirmada>>(
          `${baseUrl}/GetControles`,
        );
    
        return response.data;
      },
};