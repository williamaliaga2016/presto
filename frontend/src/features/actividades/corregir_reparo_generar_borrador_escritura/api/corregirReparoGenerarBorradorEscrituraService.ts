import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoGenerarBorradorEscritura } from "../models/corregir_reparo_generar_borrador_escritura";

export const corregirReparoGenerarBorradorEscrituraService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoGenerarBorradorEscritura | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoGenerarBorradorEscritura | null>>(
      `/api/CorregirReparoGenerarBorradorEscritura/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: CorregirReparoGenerarBorradorEscritura,
  ): Promise<ApiResponse<CorregirReparoGenerarBorradorEscritura>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoGenerarBorradorEscritura>>(
      `/api/CorregirReparoGenerarBorradorEscritura/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoGenerarBorradorEscritura/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
