import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarFirmaApoderadoBanco } from '../models/registrar_firma_apoderado_banco';

const baseUrl = '/api/RegistrarFirmaApoderadoBanco';

export const registrarFirmaApoderadoBancoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RegistrarFirmaApoderadoBanco | null>> {
    const response = await axiosClient.get<ApiResponse<RegistrarFirmaApoderadoBanco | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: RegistrarFirmaApoderadoBanco,
  ): Promise<ApiResponse<RegistrarFirmaApoderadoBanco>> {
    const response = await axiosClient.post<ApiResponse<RegistrarFirmaApoderadoBanco>>(
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