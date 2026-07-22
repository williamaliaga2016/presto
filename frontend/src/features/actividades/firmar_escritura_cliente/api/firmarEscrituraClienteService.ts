import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ControlesFirmarEscritura,
  FirmarEscrituraCliente,
} from '../models/firmar_escritura_cliente';

const PATH_URL = '/api/firmar-escritura-cliente';

export const firmarEscrituraClienteService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<FirmarEscrituraCliente | null>> {
    const response = await axiosClient.get<ApiResponse<FirmarEscrituraCliente | null>>(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },

  async guardar(
    payload: FirmarEscrituraCliente,
  ): Promise<ApiResponse<FirmarEscrituraCliente>> {
    const response = await axiosClient.post<ApiResponse<FirmarEscrituraCliente>>(`${PATH_URL}/Save`, payload);
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.post<ApiResponse<boolean>>(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesFirmarEscritura>> {
    const response = await axiosClient.get<ApiResponse<ControlesFirmarEscritura>>(`${PATH_URL}/controles`);
    return response.data;
  },
};
