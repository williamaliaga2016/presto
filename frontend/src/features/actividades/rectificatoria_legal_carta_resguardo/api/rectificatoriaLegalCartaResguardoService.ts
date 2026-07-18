import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCartaResguardo } from "../models/rectificatoria_legal_carta_resguardo";

export const rectificatoriaLegalCartaResguardoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaLegalCartaResguardo | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaLegalCartaResguardo | null>>(
      `/api/RectificatoriaLegalCartaResguardo/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RectificatoriaLegalCartaResguardo,
  ): Promise<ApiResponse<RectificatoriaLegalCartaResguardo>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaLegalCartaResguardo>>(
      `/api/RectificatoriaLegalCartaResguardo/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RectificatoriaLegalCartaResguardo/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
