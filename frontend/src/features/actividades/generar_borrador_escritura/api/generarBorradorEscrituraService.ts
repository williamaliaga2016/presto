// src/features/generar_borrador_escritura/api/generarBorradorEscrituraService.ts

import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarBorradorEscritura } from '../models/generarBorradorEscritura';

const baseUrl = '/api/GenerarBorradorEscritura';

export const generarBorradorEscrituraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GenerarBorradorEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<GenerarBorradorEscritura | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: GenerarBorradorEscritura,
  ): Promise<ApiResponse<GenerarBorradorEscritura>> {
    const response = await axiosClient.post<ApiResponse<GenerarBorradorEscritura>>(
      `${baseUrl}/Save`,
      payload
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `${baseUrl}/Avanzar/${id_expediente}`
    );
    return response.data;
  },
};