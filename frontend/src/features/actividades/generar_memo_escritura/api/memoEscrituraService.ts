import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarMemoEscritura } from '../models/generar_memo_escritura';
import type {
  MemoEscrituraData,
  MemoEscrituraRequest,
  MemoEscrituraVersion,
} from '../models/memo_escritura';

export const memoEscrituraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GenerarMemoEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<GenerarMemoEscritura | null>>(
      `/api/GenerarMemoEscritura/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: GenerarMemoEscritura,
  ): Promise<ApiResponse<GenerarMemoEscritura>> {
    const response = await axiosClient.post<ApiResponse<GenerarMemoEscritura>>(
      `/api/GenerarMemoEscritura/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.get<ApiResponse<unknown>>(
      `/api/GenerarMemoEscritura/Avanzar/${id_expediente}`,
    );
    return response.data;
  },

  async getDatosMemo(
    id_expediente: number,
  ): Promise<ApiResponse<MemoEscrituraData>> {
    const response = await axiosClient.get<ApiResponse<MemoEscrituraData>>(
      `/api/GenerarMemoEscritura/GetDatosMemo/${id_expediente}`,
    );
    return response.data;
  },

  async listarVersiones(
    id_expediente: number,
  ): Promise<ApiResponse<MemoEscrituraVersion[]>> {
    const response = await axiosClient.get<ApiResponse<MemoEscrituraVersion[]>>(
      `/api/GenerarMemoEscritura/ListarVersiones/${id_expediente}`,
    );
    return response.data;
  },

  async generarPdf(
    payload: MemoEscrituraRequest,
  ): Promise<ApiResponse<MemoEscrituraVersion>> {
    const response = await axiosClient.post<ApiResponse<MemoEscrituraVersion>>(
      `/api/GenerarMemoEscritura/GenerarPdf`,
      payload,
    );
    return response.data;
  },
};
