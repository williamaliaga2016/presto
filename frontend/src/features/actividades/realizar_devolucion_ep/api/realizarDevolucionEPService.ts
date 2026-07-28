import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarDevolucionEP, GetByExpedienteResponse } from '../models/realizar_devolucion_ep';

const PATH_URL = '/api/realizar-devolucion-ep';

export const realizarDevolucionEPService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },
  async getControles(): Promise<ApiResponse<any>> {
    const response = await axiosClient.get<ApiResponse<any>>(`${PATH_URL}/controles`);
    return response.data;
  },
  async guardar(payload: RealizarDevolucionEP): Promise<ApiResponse<RealizarDevolucionEP>> {
    const response = await axiosClient.post<ApiResponse<RealizarDevolucionEP>>(`${PATH_URL}/Save`, payload);
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },
};
