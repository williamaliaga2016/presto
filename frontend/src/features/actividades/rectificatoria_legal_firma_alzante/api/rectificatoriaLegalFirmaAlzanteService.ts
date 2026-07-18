import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalFirmaAlzante } from "../models/rectificatoria_legal_firma_alzante";

export const rectificatoriaLegalFirmaAlzanteService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaLegalFirmaAlzante | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaLegalFirmaAlzante | null>>(
      `/api/RectificatoriaLegalFirmaAlzante/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RectificatoriaLegalFirmaAlzante,
  ): Promise<ApiResponse<RectificatoriaLegalFirmaAlzante>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaLegalFirmaAlzante>>(
      `/api/RectificatoriaLegalFirmaAlzante/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RectificatoriaLegalFirmaAlzante/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
