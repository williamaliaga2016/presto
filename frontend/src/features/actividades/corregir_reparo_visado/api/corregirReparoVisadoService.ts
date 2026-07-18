import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoVisado } from "../models/corregir_reparo_visado";

export const corregirReparoVisadoService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoVisado | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoVisado | null>>(
      `/api/CorregirReparoVisado/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: CorregirReparoVisado,
  ): Promise<ApiResponse<CorregirReparoVisado>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoVisado>>(
      `/api/CorregirReparoVisado/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoVisado/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
