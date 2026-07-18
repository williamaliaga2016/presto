import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCopiasEscrituras } from "../models/corregir_reparo_copias_escrituras";

export const corregirReparoCopiasEscriturasService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoCopiasEscrituras | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoCopiasEscrituras | null>>(
      `/api/CorregirReparoCopiasEscrituras/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoCopiasEscrituras,
  ): Promise<ApiResponse<CorregirReparoCopiasEscrituras>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoCopiasEscrituras>>(
      `/api/CorregirReparoCopiasEscrituras/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoCopiasEscrituras/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
