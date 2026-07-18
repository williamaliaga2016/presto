import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';

const BASE_URL = '/api/CartaAprobacionBbva';

export interface CartaAprobacionBbva {
  id: number;
  id_expediente: number;
  id_tipo_sub_producto: string;
  modelo_carta: number;
  nombre_archivo_docx: string | null;
  nombre_archivo_pdf: string | null;
  url_docx: string | null;
  url_pdf: string | null;
  estado: string;
  error_detalle: string | null;
  version: number;
  created_date: string | null;
  modified_date: string | null;
}

export const cartaAprobacionBbvaService = {
  async getByExpediente(idExpediente: number): Promise<ApiResponse<CartaAprobacionBbva | null>> {
    const response = await axiosClient.get<ApiResponse<CartaAprobacionBbva | null>>(
      `${BASE_URL}/${idExpediente}`,
    );
    return response.data;
  },

  async getHistorico(idExpediente: number): Promise<ApiResponse<CartaAprobacionBbva[]>> {
    const response = await axiosClient.get<ApiResponse<CartaAprobacionBbva[]>>(
      `${BASE_URL}/${idExpediente}/historico`,
    );
    return response.data;
  },

  async generar(idExpediente: number): Promise<ApiResponse<null>> {
    const response = await axiosClient.get<ApiResponse<null>>(
      `${BASE_URL}/${idExpediente}/generar`,
    );
    return response.data;
  },
};
