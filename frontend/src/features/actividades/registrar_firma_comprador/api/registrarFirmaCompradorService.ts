import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmaComprador } from '../models/registrar_firma_comprador';
import type { FirmaCompradorDetalle } from '../models/registrar_firma_comprador_detalle';

export interface FirmaCompradorRequest extends FirmaComprador {
  firma_comprador_detalle: FirmaCompradorDetalle[];
}

export const registrarFirmaCompradorService = {
  async getByIdExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<FirmaCompradorRequest | null>> {
    const response = await axiosClient.get<
      ApiResponse<FirmaCompradorRequest | null>
    >(
      `/api/RegistrarFirmaComprador/GetByIdExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: FirmaCompradorRequest,
  ): Promise<ApiResponse<FirmaCompradorRequest>> {
    const response = await axiosClient.post<
      ApiResponse<FirmaCompradorRequest>
    >(`/api/RegistrarFirmaComprador/Save`, payload);

    return response.data;
  },

  
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RegistrarFirmaComprador/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};