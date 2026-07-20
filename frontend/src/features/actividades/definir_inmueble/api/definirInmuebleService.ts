import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesValidarInformacion } from
  '../../validar_informacion/models/catalogo';
import type {
  DefinirInmuebleAvanzarResponse,
  DefinirInmuebleBBVA,
} from '../models/definir_inmueble';

const baseUrl = '/api/definir-inmueble';

export const definirInmuebleService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<DefinirInmuebleBBVA | null>> {
    const response = await axiosClient.get<ApiResponse<DefinirInmuebleBBVA | null>>(
      `${baseUrl}/${id_expediente}`,
    );
    return response.data;
  },

  async getControles(): Promise<ApiResponse<ControlesValidarInformacion>> {
    const response = await axiosClient.get<ApiResponse<ControlesValidarInformacion>>(
      `${baseUrl}/controles`,
    );
    return response.data;
  },

  async guardar(
    payload: DefinirInmuebleBBVA,
  ): Promise<ApiResponse<DefinirInmuebleBBVA>> {
    const response = await axiosClient.post<ApiResponse<DefinirInmuebleBBVA>>(
      `${baseUrl}/guardar`,
      payload,
    );
    return response.data;
  },

  async avanzar(
    id_expediente: number,
    confirmar = false,
  ): Promise<ApiResponse<DefinirInmuebleAvanzarResponse>> {
    const response = await axiosClient.post<
      ApiResponse<DefinirInmuebleAvanzarResponse>
    >(`${baseUrl}/${id_expediente}/avanzar`, { confirmar });
    return response.data;
  },
};
