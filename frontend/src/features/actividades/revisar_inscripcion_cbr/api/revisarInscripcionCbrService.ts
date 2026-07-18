import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarInscripcionCbr } from '../models/revisar_inscripcion_cbr';

export const revisarInscripcionCbrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarInscripcionCbr | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarInscripcionCbr | null>>(
      `/api/RevisarInscripcionCbr/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RevisarInscripcionCbr,
  ): Promise<ApiResponse<RevisarInscripcionCbr>> {
    const response = await axiosClient.post<ApiResponse<RevisarInscripcionCbr>>(
      `/api/RevisarInscripcionCbr/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RevisarInscripcionCbr/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
