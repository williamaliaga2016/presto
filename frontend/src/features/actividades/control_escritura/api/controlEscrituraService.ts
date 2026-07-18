import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlEscritura } from '../models/control_escritura';

export const controlEscrituraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ControlEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<ControlEscritura | null>>(
      `/api/ControlEscritura/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },
  async save(
    payload: ControlEscritura,
  ): Promise<ApiResponse<ControlEscritura>> {
    const response = await axiosClient.post<ApiResponse<ControlEscritura>>(
      `/api/ControlEscritura/Save`,
      payload,
    );
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/ControlEscritura/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
