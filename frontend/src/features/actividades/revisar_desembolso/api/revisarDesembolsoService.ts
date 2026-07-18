import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarDesembolso } from '../models/revisar_desembolso';

export const revisarDesembolsoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDesembolso | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarDesembolso | null>>(
      `/api/RevisarDesembolso/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },
  async save(
    payload: RevisarDesembolso,
  ): Promise<ApiResponse<RevisarDesembolso>> {
    const response = await axiosClient.post<ApiResponse<RevisarDesembolso>>(
      `/api/RevisarDesembolso/Save`,
      payload,
    );
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RevisarDesembolso/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};