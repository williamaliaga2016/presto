import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirControlEscritura } from "../models/corregir_control_escritura";

export const corregirControlEscrituraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirControlEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirControlEscritura | null>>(
      `/api/CorregirControlEscritura/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: CorregirControlEscritura,
  ): Promise<ApiResponse<CorregirControlEscritura>> {
    const response = await axiosClient.post<ApiResponse<CorregirControlEscritura>>(
      `/api/CorregirControlEscritura/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirControlEscritura/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
