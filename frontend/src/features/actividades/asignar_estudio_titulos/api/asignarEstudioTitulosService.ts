import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type { AsignarEstudioTitulos } from '../models/asignar_estudio_titulos';

export const asignarEstudioTitulosService = {
  async getByIdExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<AsignarEstudioTitulos | null>> {
    const response = await axiosClient.get<ApiResponse<AsignarEstudioTitulos | null>>(
      `/api/AsignarEstudioTitulos/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: AsignarEstudioTitulos,
  ): Promise<ApiResponse<AsignarEstudioTitulos>> {
    const response = await axiosClient.post<ApiResponse<AsignarEstudioTitulos>>(
      '/api/AsignarEstudioTitulos/Save',
      payload,
    );
    return response.data;
  },

  async getCatalogoAbogado(): Promise<ApiResponse<ControlBaseDTO[]>> {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      '/api/AsignarEstudioTitulos/GetCatalogoAbogado',
    );
    return response.data;
  },
};

