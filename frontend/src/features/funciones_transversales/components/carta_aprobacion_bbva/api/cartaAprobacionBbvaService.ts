import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";

const baseUrl = "/api/CartaAprobacionBbva";

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
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CartaAprobacionBbva | null>> {
    const response = await axiosClient.get<
      ApiResponse<CartaAprobacionBbva | null>
    >(`${baseUrl}/GetByExpediente/${id_expediente}`);

    return response.data;
  },

  async getHistorico(
    id_expediente: number,
  ): Promise<ApiResponse<CartaAprobacionBbva[]>> {
    const response = await axiosClient.get<
      ApiResponse<CartaAprobacionBbva[]>
    >(`${baseUrl}/Historico/${id_expediente}`);

    return response.data;
  },

  async generar(
    id_expediente: number,
  ): Promise<ApiResponse<null>> {
    const response = await axiosClient.get<ApiResponse<null>>(
      `${baseUrl}/Generar/${id_expediente}`,
    );

    return response.data;
  },
};
