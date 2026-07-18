import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarEstudioTitulos } from '../models/generar_estudio_titulos';

export const generarEstudioTitulosService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GenerarEstudioTitulos | null>> {
    const response = await axiosClient.get<ApiResponse<GenerarEstudioTitulos | null>>(
      `/api/GenerarEstudioTitulos/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },
  async save(payload: GenerarEstudioTitulos): Promise<ApiResponse<GenerarEstudioTitulos>> {
    const response = await axiosClient.post<ApiResponse<GenerarEstudioTitulos>>(
      `/api/GenerarEstudioTitulos/Save`,
      payload,
    );
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/GenerarEstudioTitulos/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};