import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import type { RevisarDocumentosInmueble } from '../models/revisar_documentos_inmueble';

const baseUrl = '/api/RevisarDocumentosInmueble';

export const revisarDocumentosInmuebleService = {
  async getByExpediente(id_expediente: number): Promise<ApiResponse<RevisarDocumentosInmueble | null>> {
    const response = await axiosClient.get<ApiResponse<RevisarDocumentosInmueble | null>>(
      `${baseUrl}/${id_expediente}`,
    );
    return response.data;
  },

  async guardar(payload: RevisarDocumentosInmueble): Promise<ApiResponse<RevisarDocumentosInmueble>> {
    const response = await axiosClient.post<ApiResponse<RevisarDocumentosInmueble>>(
      `${baseUrl}/guardar`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.post<ApiResponse<unknown>>(
      `${baseUrl}/${id_expediente}/avanzar`,
    );
    return response.data;
  },

  async getControles(id_expediente: number): Promise<ApiResponse<{ motivo_devolucion: ControlBaseDTO[] }>> {
    const response = await axiosClient.get<ApiResponse<{ motivo_devolucion: ControlBaseDTO[] }>>(
      `${baseUrl}/${id_expediente}/controles`,
    );
    return response.data;
  },
};
