import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCierreCopias } from "../models/rectificatoria_legal_cierre_copias";

export const rectificatoriaLegalCierreCopiasService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaLegalCierreCopias | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaLegalCierreCopias | null>>(
      `/api/RectificatoriaLegalCierreCopias/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: RectificatoriaLegalCierreCopias,
  ): Promise<ApiResponse<RectificatoriaLegalCierreCopias>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaLegalCierreCopias>>(
      `/api/RectificatoriaLegalCierreCopias/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RectificatoriaLegalCierreCopias/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
