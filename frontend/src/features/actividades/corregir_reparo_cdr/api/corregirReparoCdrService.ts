import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCdr } from "../models/corregir_reparo_cdr";

export const corregirReparoCdrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoCdr | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoCdr | null>>(
      `/api/CorregirReparoCdr/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: CorregirReparoCdr,
  ): Promise<ApiResponse<CorregirReparoCdr>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoCdr>>(
      `/api/CorregirReparoCdr/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoCdr/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
