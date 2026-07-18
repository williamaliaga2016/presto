import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarIntegracionDocumentosData } from '../models/validar_integracion_documentos';
import type { ValidarIntegracionCatalogos } from '../models/catalogo';
import type {
  AvanzarValidarIntegracionResponse,
  ValidarIntegracionApiDetail,
} from '../models/validar_integracion_documentos.response';

export const validarIntegracionDocumentosService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidarIntegracionApiDetail | null>> {
    const response = await axiosClient.get<
      ApiResponse<ValidarIntegracionApiDetail | null>
    >(`/api/ValidarIntegracion/${id_expediente}`);

    return response.data;
  },

  async getControles(
    id_expediente: number,
  ): Promise<ApiResponse<ValidarIntegracionCatalogos>> {
    const response = await axiosClient.get<
      ApiResponse<ValidarIntegracionCatalogos>
    >(`/api/ValidarIntegracion/${id_expediente}/controles`);

    return response.data;
  },

  async save(
    payload: ValidarIntegracionDocumentosData,
  ): Promise<ApiResponse<ValidarIntegracionDocumentosData>> {
    const response = await axiosClient.post<
      ApiResponse<ValidarIntegracionDocumentosData>
    >('/api/ValidarIntegracion/guardar', payload);

    return response.data;
  },

  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<AvanzarValidarIntegracionResponse>> {
    const response = await axiosClient.post<
      ApiResponse<AvanzarValidarIntegracionResponse>
    >(`/api/ValidarIntegracion/${id_expediente}/avanzar`, {});

    return response.data;
  },
};
