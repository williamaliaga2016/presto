import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarRevisionPrevioFirmaBanco } from '../models/realizar_revision_previo_firma_banco';

export const realizarRevisionPrevioFirmaBancoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RealizarRevisionPrevioFirmaBanco | null>> {
    const response = await axiosClient.get<ApiResponse<RealizarRevisionPrevioFirmaBanco | null>>(
      `/api/RealizarRevisionPrevioFirmaBanco/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RealizarRevisionPrevioFirmaBanco,
  ): Promise<ApiResponse<RealizarRevisionPrevioFirmaBanco>> {
    const response = await axiosClient.post<ApiResponse<RealizarRevisionPrevioFirmaBanco>>(
      `/api/RealizarRevisionPrevioFirmaBanco/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RealizarRevisionPrevioFirmaBanco/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
