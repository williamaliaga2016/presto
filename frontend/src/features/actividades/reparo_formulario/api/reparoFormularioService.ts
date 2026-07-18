import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { ReparoFormulario } from "../models/reparo_formulario";

export const reparoFormularioService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ReparoFormulario | null>> {
    const response = await axiosClient.get<ApiResponse<ReparoFormulario | null>>(
      `/api/ReparoFormulario/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: ReparoFormulario,
  ): Promise<ApiResponse<ReparoFormulario>> {
    const response = await axiosClient.post<ApiResponse<ReparoFormulario>>(
      `/api/ReparoFormulario/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/ReparoFormulario/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
