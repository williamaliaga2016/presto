import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarEscrituraCbr } from '../models/registrar_escritura_cbr';
import { ControlesRegistrarEscrituraCbr } from '../models/catalogo';

export const registrarEscrituraCbrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RegistrarEscrituraCbr | null>> {
    const response = await axiosClient.get<ApiResponse<RegistrarEscrituraCbr | null>>(
      `/api/RegistrarEscrituraCbr/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RegistrarEscrituraCbr,
  ): Promise<ApiResponse<RegistrarEscrituraCbr>> {
    const response = await axiosClient.post<ApiResponse<RegistrarEscrituraCbr>>(
      `/api/RegistrarEscrituraCbr/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RegistrarEscrituraCbr/Avanzar/${id_expediente}`,
    );

    return response.data;
  },

  async getControlesRegistrarEscrituraCbr(): Promise<ApiResponse<ControlesRegistrarEscrituraCbr>> {
    const response = await axiosClient.get<ApiResponse<ControlesRegistrarEscrituraCbr>>(
      `/api/RegistrarEscrituraCbr/getControlesRegistrarEscrituraCbr`,
    );

    return response.data;
  },
};
