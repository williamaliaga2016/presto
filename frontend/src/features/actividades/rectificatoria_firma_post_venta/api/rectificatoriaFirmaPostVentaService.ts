import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirmaPostVenta } from "../models/rectificatoria_firma_post_venta";
import { RectificatoriaFirmaPostVentaDetalle } from "../models/rectificatoria_firma_post_venta_detalle";

export const rectificatoriaFirmaPostVentaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaFirmaPostVenta | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaFirmaPostVenta | null>>(
      `/api/RectificatoriaFirmaPostVenta/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async getDetalleByExpediente(
    id_expediente: number,
    rol_comparecencia: string,
  ): Promise<ApiResponse<RectificatoriaFirmaPostVentaDetalle[]>> {
    const response = await axiosClient.get<
      ApiResponse<RectificatoriaFirmaPostVentaDetalle[]>
    >(
      `/api/RectificatoriaFirmaPostVenta/GetRectificatoriaPostVentaDetByExpediente/${id_expediente}/${rol_comparecencia}`,
    );

    return response.data;
  },

  async save(
    payload: RectificatoriaFirmaPostVenta,
  ): Promise<ApiResponse<RectificatoriaFirmaPostVenta>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaFirmaPostVenta>>(
      `/api/RectificatoriaFirmaPostVenta/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RectificatoriaFirma/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
