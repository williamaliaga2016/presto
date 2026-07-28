import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarVBFinalAbogado, GetByExpedienteResponse } from '../models/realizar_vb_final_abogado';
import type { ControlesVBFinalAbogado } from '../models/controles';

const PATH_URL = '/api/realizar-vb-final-abogado';

export const realizarVBFinalAbogadoService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<GetByExpedienteResponse | null>> {
    const response = await axiosClient.get<ApiResponse<GetByExpedienteResponse | null>>(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },
  async getControles(): Promise<ApiResponse<ControlesVBFinalAbogado>> {
    const response = await axiosClient.get<ApiResponse<ControlesVBFinalAbogado>>(`${PATH_URL}/controles`);
    return response.data;
  },
  async guardar(payload: RealizarVBFinalAbogado): Promise<ApiResponse<RealizarVBFinalAbogado>> {
    const response = await axiosClient.post<ApiResponse<RealizarVBFinalAbogado>>(`${PATH_URL}/Save`, payload);
    return response.data;
  },
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },
};
