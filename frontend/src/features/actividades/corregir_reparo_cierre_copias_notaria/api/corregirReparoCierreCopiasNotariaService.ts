import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCierreCopiasNotaria } from "../models/corregir_reparo_cierre_copias_notaria";

export const corregirReparoCierreCopiasNotariaService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoCierreCopiasNotaria | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoCierreCopiasNotaria | null>>(
      `/api/CorregirReparoCierreCopiasNotaria/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoCierreCopiasNotaria,
  ): Promise<ApiResponse<CorregirReparoCierreCopiasNotaria>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoCierreCopiasNotaria>>(
      `/api/CorregirReparoCierreCopiasNotaria/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoCierreCopiasNotaria/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
