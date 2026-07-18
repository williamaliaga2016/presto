import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AltaSolicitud } from '../models/alta_solicitud';

export const altaSolicitudService = {
  async getByIdExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<AltaSolicitud | null>> {
    const response = await axiosClient.get<ApiResponse<AltaSolicitud | null>>(
      `/api/AltaSolicitud/GetAltaSolicitudByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(payload: AltaSolicitud): Promise<ApiResponse<AltaSolicitud>> {
    const response = await axiosClient.post<ApiResponse<AltaSolicitud>>(
      `/api/AltaSolicitud/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/AltaSolicitud/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
