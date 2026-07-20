import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  GuardarValidarIntegracionDocumentos,
  ValidarIntegracionDocumentos,
} from '../models/validar_integracion_documentos';
import type { ValidarIntegracionCatalogos } from
  '../models/catalogo';
import type { Interviniente } from
  '../models/interviniente';

const baseUrl = '/api/ValidarIntegracion';

export const validarIntegracionDocumentosService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ValidarIntegracionDocumentos['formulario'] | null>> {
    const response = await axiosClient.get<
      ApiResponse<ValidarIntegracionDocumentos['formulario'] | null>
    >(`${baseUrl}/GetByExpediente/${id_expediente}`);

    return response.data;
  },

  async getControles(): Promise<
    ApiResponse<ValidarIntegracionCatalogos>
  > {
    const response = await axiosClient.get<
      ApiResponse<ValidarIntegracionCatalogos>
    >(`${baseUrl}/Controles`);

    return response.data;
  },

  async save(
    payload: GuardarValidarIntegracionDocumentos,
  ): Promise<ApiResponse<ValidarIntegracionDocumentos['formulario']>> {
    const response = await axiosClient.post<
      ApiResponse<ValidarIntegracionDocumentos['formulario']>
    >(`${baseUrl}/Save`, payload.formulario);

    return response.data;
  },

  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.post<
      ApiResponse<unknown>
    >(`${baseUrl}/Avanzar/${id_expediente}`);

    return response.data;
  },

  async getIntervinientes(
    id_expediente: number,
  ): Promise<ApiResponse<Interviniente[]>> {
    const response = await axiosClient.get<
      ApiResponse<Interviniente[]>
    >(
      `${baseUrl}/Intervinientes/GetIntervinientes/` +
      `${id_expediente}`,
    );

    return response.data;
  },

  async saveInterviniente(
    id_expediente: number,
    payload: Interviniente,
  ): Promise<ApiResponse<Interviniente>> {
    const response = await axiosClient.post<
      ApiResponse<Interviniente>
    >(
      `${baseUrl}/Intervinientes/GuardarInterviniente/` +
      `${id_expediente}`,
      payload,
    );

    return response.data;
  },
};
