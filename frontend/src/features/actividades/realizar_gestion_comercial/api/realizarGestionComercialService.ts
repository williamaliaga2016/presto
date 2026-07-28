import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarGestionComercial, GetByExpedienteResponse } from '../models/realizar_gestion_comercial';

const PATH_URL = '/api/realizar-gestion-comercial';

export const realizarGestionComercialService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },
  async guardar(payload: RealizarGestionComercial): Promise<ApiResponse<RealizarGestionComercial>> {
    const response = await axiosClient.post<ApiResponse<RealizarGestionComercial>>(`${PATH_URL}/Save`, payload);
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },
};
