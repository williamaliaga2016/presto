import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoInstPago } from '../models/corregir_reparo_inst_pago';

export const corregirReparoInstPagoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoInstPago | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoInstPago | null>>(
      `/api/CorregirReparoInstPago/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoInstPago,
  ): Promise<ApiResponse<CorregirReparoInstPago>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoInstPago>>(
      `/api/CorregirReparoInstPago/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoInstPago/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
