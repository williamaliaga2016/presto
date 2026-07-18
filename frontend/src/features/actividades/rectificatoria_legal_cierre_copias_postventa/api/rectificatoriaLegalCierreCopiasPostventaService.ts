import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCierreCopiasPostventa } from "../models/rectificatoria_legal_cierre_copias_postventa";

export const rectificatoriaLegalCierreCopiasPostventaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaLegalCierreCopiasPostventa | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaLegalCierreCopiasPostventa | null>>(
      `/api/RectificatoriaLegalCierreCopiasPostventa/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RectificatoriaLegalCierreCopiasPostventa,
  ): Promise<ApiResponse<RectificatoriaLegalCierreCopiasPostventa>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaLegalCierreCopiasPostventa>>(
      `/api/RectificatoriaLegalCierreCopiasPostventa/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RectificatoriaLegalCierreCopiasPostventa/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
