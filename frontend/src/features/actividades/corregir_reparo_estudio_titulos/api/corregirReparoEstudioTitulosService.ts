import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoEstudioTitulos } from "../models/corregir_reparo_estudio_titulos";

export const corregirReparoEstudioTitulosService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoEstudioTitulos | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoEstudioTitulos | null>>(
      `/api/CorregirReparoEstudioTitulos/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoEstudioTitulos,
  ): Promise<ApiResponse<CorregirReparoEstudioTitulos>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoEstudioTitulos>>(
      `/api/CorregirReparoEstudioTitulos/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.get<ApiResponse<unknown>>(
      `/api/CorregirReparoEstudioTitulos/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
