import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type {
  RevisarDocumentosInmuebleFormulario,
} from '../models/revisar_documentos_inmueble';

const baseUrl = '/api/RevisarDocumentosInmueble';

export const revisarDocumentosInmuebleService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisarDocumentosInmuebleFormulario | null>> {
    const response = await axiosClient.get<
      ApiResponse<RevisarDocumentosInmuebleFormulario | null>
    >(`${baseUrl}/${id_expediente}`);

    return response.data;
  },

  async guardar(
    payload: RevisarDocumentosInmuebleFormulario,
  ): Promise<ApiResponse<RevisarDocumentosInmuebleFormulario>> {
    const response = await axiosClient.post<
      ApiResponse<RevisarDocumentosInmuebleFormulario>
    >(`${baseUrl}/guardar`, payload);

    return response.data;
  },

  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.post<ApiResponse<unknown>>(
      `${baseUrl}/${id_expediente}/avanzar`,
    );

    return response.data;
  },

  async getControles(
    id_expediente: number,
  ): Promise<
    ApiResponse<{ motivo_devolucion: ControlBaseDTO[] }>
  > {
    const response = await axiosClient.get<
      ApiResponse<{ motivo_devolucion: ControlBaseDTO[] }>
    >(`${baseUrl}/${id_expediente}/controles`);

    return response.data;
  },
};
