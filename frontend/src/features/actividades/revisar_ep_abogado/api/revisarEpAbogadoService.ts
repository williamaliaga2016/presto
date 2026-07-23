import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesRevisarEp,
  RevisarEpAbogado,
} from '../models/revisar_ep_abogado';

const PATH_URL = '/api/revisar-ep-abogado';

export const revisarEpAbogadoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarEpAbogado | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarEpAbogado | null>>(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },

  async guardar(
    payload: RevisarEpAbogado,
  ): Promise<ApiResponse<RevisarEpAbogado>> {
    const response = await axiosClient.post<ApiResponse<RevisarEpAbogado>>(`${PATH_URL}/Save`, payload);
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },

  async getControles(id_expediente: number): Promise<ApiResponse<ControlesRevisarEp>> {
    const response = await axiosClient.get<ApiResponse<ControlesRevisarEp>>(`${PATH_URL}/controles/${id_expediente}`);
    return response.data;
  },
};
