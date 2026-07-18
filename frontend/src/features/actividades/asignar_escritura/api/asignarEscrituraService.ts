import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type { AsignarEscritura } from '../models/asignar_escritura';

export const asignarEscrituraService = {
  async getByIdExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<AsignarEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<AsignarEscritura | null>>(
      `/api/AsignarEscritura/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: AsignarEscritura,
  ): Promise<ApiResponse<AsignarEscritura>> {
    const response = await axiosClient.post<ApiResponse<AsignarEscritura>>(
      '/api/AsignarEscritura/Save',
      payload,
    );
    return response.data;
  },

  async getCatalogoAbogado(): Promise<ApiResponse<ControlBaseDTO[]>> {
    const response = await axiosClient.get<ApiResponse<ControlBaseDTO[]>>(
      '/api/AsignarEscritura/GetCatalogoAbogado',
    );
    return response.data;
  },

    async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
      const response = await axiosClient.get<ApiResponse<boolean>>(
        `/api/AsignarEscritura/Avanzar/${id_expediente}`
      );
      return response.data;
    },
};
