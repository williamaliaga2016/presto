import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarEntregaEpFirmada, GetByExpedienteResponse } from '../models/realizar_entrega_ep_firmada';

const PATH_URL = '/api/realizar-entrega-ep-firmada';

export const realizarEntregaEpFirmadaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(
      `${PATH_URL}/GetByIdExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(
    payload: RealizarEntregaEpFirmada,
  ): Promise<ApiResponse<RealizarEntregaEpFirmada>> {
    const response = await axiosClient.post<ApiResponse<RealizarEntregaEpFirmada>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(
      `${PATH_URL}/avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
