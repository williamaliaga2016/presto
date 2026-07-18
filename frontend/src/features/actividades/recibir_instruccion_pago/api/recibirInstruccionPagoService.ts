import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecibirInstruccionPago } from '../models/recibir_instruccion_pago';
import { ControlesRecibirInstruccionPago } from '../models/catalogo';

export const recibirInstruccionPagoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RecibirInstruccionPago | null>> {
    const response = await axiosClient.get<ApiResponse<RecibirInstruccionPago | null>>(
      `/api/RecibirInstruccionPago/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RecibirInstruccionPago,
  ): Promise<ApiResponse<RecibirInstruccionPago>> {
    const response = await axiosClient.post<ApiResponse<RecibirInstruccionPago>>(
      `/api/RecibirInstruccionPago/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RecibirInstruccionPago/Avanzar/${id_expediente}`,
    );

    return response.data;
  },

  async getControlesRecibirInstruccionPago(): Promise<ApiResponse<ControlesRecibirInstruccionPago>> {
    const response = await axiosClient.get<ApiResponse<ControlesRecibirInstruccionPago>>(
      `/api/RecibirInstruccionPago/getControlesRecibirInstruccionPago`,
    );

    return response.data;
  },
};
