import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirCartaResguardo } from "../models/corregir_carta_resguardo";

export const corregirCartaResguardoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirCartaResguardo | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirCartaResguardo | null>>(
      `/api/CorregirCartaResguardo/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: CorregirCartaResguardo,
  ): Promise<ApiResponse<CorregirCartaResguardo>> {
    const response = await axiosClient.post<ApiResponse<CorregirCartaResguardo>>(
      `/api/CorregirCartaResguardo/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirCartaResguardo/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
