import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirma } from "../models/rectificatoria_firma";
import { RectificatoriaFirmaDetalle } from "../models/rectificatoria_firma_detalle";

export const rectificatoriaFirmaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaFirma | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaFirma | null>>(
      `/api/RectificatoriaFirma/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async getDetalleByExpediente(
    id_expediente: number,
    rol_comparecencia: string,
  ): Promise<ApiResponse<RectificatoriaFirmaDetalle[]>> {
    const response = await axiosClient.get<
      ApiResponse<RectificatoriaFirmaDetalle[]>
    >(
      `/api/RectificatoriaFirma/GetRectificatoriaDetByExpediente/${id_expediente}/${rol_comparecencia}`,
    );

    return response.data;
  },

  async save(
    payload: RectificatoriaFirma,
  ): Promise<ApiResponse<RectificatoriaFirma>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaFirma>>(
      `/api/RectificatoriaFirma/Save`,
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
