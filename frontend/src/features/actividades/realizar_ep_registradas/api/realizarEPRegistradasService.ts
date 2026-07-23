import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarEPRegistradas, GetByExpedienteResponse } from '../models/realizar_ep_registradas';
import type { ControlesEPRegistradas } from '../models/controles';

const PATH_URL = '/api/realizar-ep-registradas';

export const realizarEPRegistradasService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },
  async getControles(): Promise<ApiResponse<ControlesEPRegistradas>> {
    const response = await axiosClient.get<ApiResponse<ControlesEPRegistradas>>(`${PATH_URL}/controles`);
    return response.data;
  },
  async guardar(payload: RealizarEPRegistradas): Promise<ApiResponse<RealizarEPRegistradas>> {
    const response = await axiosClient.post<ApiResponse<RealizarEPRegistradas>>(`${PATH_URL}/Save`, payload);
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },
};
