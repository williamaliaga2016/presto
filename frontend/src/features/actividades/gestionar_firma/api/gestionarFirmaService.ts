import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesGestionarFirma } from '../models/controles';
import type { GestionarFirma } from '../models/gestionar_firma';

export const gestionarFirmaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GestionarFirma | null>> {
    const response = await axiosClient.get<ApiResponse<GestionarFirma | null>>(
      `/api/gestionar-firma/${id_expediente}`,
    );

    return response.data;
  },

  async guardar(payload: GestionarFirma): Promise<ApiResponse<GestionarFirma>> {
    const response = await axiosClient.post<ApiResponse<GestionarFirma>>(
      `/api/gestionar-firma/guardar`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `/api/gestionar-firma/${id_expediente}/avanzar`,
    );

    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesGestionarFirma>> {
    const response = await axiosClient.get<ApiResponse<ControlesGestionarFirma>>(
      `/api/gestionar-firma/controles`,
    );

    return response.data;
  },
};
