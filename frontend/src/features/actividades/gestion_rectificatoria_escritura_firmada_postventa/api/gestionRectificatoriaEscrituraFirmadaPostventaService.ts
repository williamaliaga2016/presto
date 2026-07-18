import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaEscrituraFirmadaPostventa } from '../models/gestion_rectificatoria_escritura_firmada_postventa';
import type { ControlesGestionRectificatoriaEscrituraFirmadaPostventa } from '../models/catalogo';

const baseUrl = '/api/GestionRectificatoriaEscrituraFirmadaPostventa';

export const GestionRectificatoriaEscrituraFirmadaPostventaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GestionRectificatoriaEscrituraFirmadaPostventa | null>> {
    const response = await axiosClient.get<ApiResponse<GestionRectificatoriaEscrituraFirmadaPostventa | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: GestionRectificatoriaEscrituraFirmadaPostventa,
  ): Promise<ApiResponse<GestionRectificatoriaEscrituraFirmadaPostventa>> {
    const response = await axiosClient.post<ApiResponse<GestionRectificatoriaEscrituraFirmadaPostventa>>(
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

    async getControles(): Promise<ApiResponse<ControlesGestionRectificatoriaEscrituraFirmadaPostventa>> {
          const response = await axiosClient.get<ApiResponse<ControlesGestionRectificatoriaEscrituraFirmadaPostventa>>(
            `${baseUrl}/GetControles`,
          );
      
          return response.data;
        },
};