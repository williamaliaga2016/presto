import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmaVendedor } from '../models/registrar_firma_vendedor';
import type { FirmaVendedorDetalle } from '../models/registrar_firma_vendedor_detalle';

export interface FirmaVendedorRequest extends FirmaVendedor {
  firma_vendedor_detalle: FirmaVendedorDetalle[];
}

export const registrarFirmaVendedorService = {
  async getByIdExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<FirmaVendedorRequest | null>> {
    const response = await axiosClient.get<
      ApiResponse<FirmaVendedorRequest | null>
    >(
      `/api/RegistrarFirmaVendedor/GetByIdExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: FirmaVendedorRequest,
  ): Promise<ApiResponse<FirmaVendedorRequest>> {
    const response = await axiosClient.post<
      ApiResponse<FirmaVendedorRequest>
    >(
      `/api/RegistrarFirmaVendedor/Save`,
      payload,
    );

    return response.data;
  },

  
  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RegistrarFirmaVendedor/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};