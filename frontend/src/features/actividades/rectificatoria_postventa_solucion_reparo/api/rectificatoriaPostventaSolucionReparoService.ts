import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaPostventaSolucionReparo } from "../models/rectificatoria_postventa_solucion_reparo";

export const rectificatoriaPostventaSolucionReparoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RectificatoriaPostventaSolucionReparo | null>> {
    const response = await axiosClient.get<ApiResponse<RectificatoriaPostventaSolucionReparo | null>>(
      `/api/RectificatoriaPostventaSolucionReparo/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RectificatoriaPostventaSolucionReparo,
  ): Promise<ApiResponse<RectificatoriaPostventaSolucionReparo>> {
    const response = await axiosClient.post<ApiResponse<RectificatoriaPostventaSolucionReparo>>(
      `/api/RectificatoriaPostventaSolucionReparo/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RectificatoriaPostventaSolucionReparo/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
