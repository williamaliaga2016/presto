import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesGestionarFirmaFisica } from '../models/controles';
import type {
  GestionarFirmaFisica,
} from '../models/gestionar_firma_fisica';

export const gestionarFirmaFisicaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GestionarFirmaFisica | null>> {
    const response = await axiosClient.get<
      ApiResponse<GestionarFirmaFisica | null>
    >(`/api/gestionar-firma-fisica/${id_expediente}`);

    return response.data;
  },

  async guardar(
    payload: GestionarFirmaFisica,
  ): Promise<ApiResponse<GestionarFirmaFisica>> {
    const response = await axiosClient.post<
      ApiResponse<GestionarFirmaFisica>
    >(`/api/gestionar-firma-fisica/guardar`, payload);

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `/api/gestionar-firma-fisica/${id_expediente}/avanzar`,
    );

    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesGestionarFirmaFisica>> {
    const response = await axiosClient.get<
      ApiResponse<ControlesGestionarFirmaFisica>
    >(`/api/gestionar-firma-fisica/controles`);

    return response.data;
  },
};
