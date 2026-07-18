import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoDatosOperacion } from "../models/corregir_reparo_datos_operacion";

export const corregirReparoDatosOperacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoDatosOperacion | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoDatosOperacion | null>>(
      `/api/CorregirReparoDatosOperacion/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoDatosOperacion,
  ): Promise<ApiResponse<CorregirReparoDatosOperacion>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoDatosOperacion>>(
      `/api/CorregirReparoDatosOperacion/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoDatosOperacion/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
