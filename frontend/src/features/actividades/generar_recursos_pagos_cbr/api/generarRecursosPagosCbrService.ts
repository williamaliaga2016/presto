import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarRecursosPagosCbr } from '../models/generar_recursos_pagos_cbr';

export const generarRecursosPagosCbrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<GenerarRecursosPagosCbr | null>> {
    const response = await axiosClient.get<ApiResponse<GenerarRecursosPagosCbr | null>>(
      `/api/GenerarRecursosPagosCbr/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: GenerarRecursosPagosCbr,
  ): Promise<ApiResponse<GenerarRecursosPagosCbr>> {
    const response = await axiosClient.post<ApiResponse<GenerarRecursosPagosCbr>>(
      `/api/GenerarRecursosPagosCbr/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/GenerarRecursosPagosCbr/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
