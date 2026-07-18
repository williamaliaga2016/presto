import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCalculoDoc } from "../models/corregir_reparo_calculo_doc";
import type { ControlesPropiedad } from "../models/catalogo";

export const corregirReparoCalculoDocService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CorregirReparoCalculoDoc | null>> {
    const response = await axiosClient.get<ApiResponse<CorregirReparoCalculoDoc | null>>(
      `/api/CorregirReparoCalculoDoc/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: CorregirReparoCalculoDoc,
  ): Promise<ApiResponse<CorregirReparoCalculoDoc>> {
    const response = await axiosClient.post<ApiResponse<CorregirReparoCalculoDoc>>(
      `/api/CorregirReparoCalculoDoc/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/CorregirReparoCalculoDoc/Avanzar/${id_expediente}`,
    );
    return response.data;
  },

  async getControlesPropiedad(): Promise<ApiResponse<ControlesPropiedad>> {
    const response = await axiosClient.get<ApiResponse<ControlesPropiedad>>(
      "/api/DatosOperacion/GetControlesPropiedad",
    );
    return response.data;
  },
};
