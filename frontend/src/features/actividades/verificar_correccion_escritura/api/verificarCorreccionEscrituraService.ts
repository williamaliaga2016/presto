// src/features/generar_borrador_escritura/api/generarBorradorEscrituraService.ts

import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarCorreccionEscritura } from '../models/verificar_correccion_escritura';

const baseUrl = '/api/VerificarCorreccionEscritura';

export const verificarCorreccionEscrituraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<VerificarCorreccionEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<VerificarCorreccionEscritura | null>>(
      `${baseUrl}/GetByExpediente/${id_expediente}`
    );
    return response.data;
  },

  async save(
    payload: VerificarCorreccionEscritura,
  ): Promise<ApiResponse<VerificarCorreccionEscritura>> {
    const response = await axiosClient.post<ApiResponse<VerificarCorreccionEscritura>>(
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