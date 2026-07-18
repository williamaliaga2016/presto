import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoGenerarMemoEscritura } from "../models/corregir_reparo_generar_memo_escritura";

export const corregirReparoGenerarMemoEscrituraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoGenerarMemoEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoGenerarMemoEscritura | null>>(
      `/api/CorregirReparoGenerarMemoEscritura/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoGenerarMemoEscritura,
  ): Promise<ApiResponse<CorregirReparoGenerarMemoEscritura>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoGenerarMemoEscritura>>(
      `/api/CorregirReparoGenerarMemoEscritura/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoGenerarMemoEscritura/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
