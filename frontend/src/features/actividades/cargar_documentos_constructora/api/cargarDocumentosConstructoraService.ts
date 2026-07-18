import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CargarDocumentosConstructora } from '../models/cargar_documentos_constructora';

const baseUrl = '/api/CargarDocumentosConstructora';

export const cargarDocumentosConstructoraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargarDocumentosConstructora | null>> {
    const response = await axiosClient.get<ApiResponse<CargarDocumentosConstructora | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(
    payload: CargarDocumentosConstructora,
  ): Promise<ApiResponse<CargarDocumentosConstructora>> {
    const response = await axiosClient.post<ApiResponse<CargarDocumentosConstructora>>(
      `${baseUrl}/Guardar`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.post<ApiResponse<unknown>>(
      `${baseUrl}/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
