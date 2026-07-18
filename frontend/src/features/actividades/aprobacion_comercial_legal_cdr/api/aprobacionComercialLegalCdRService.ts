import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { AprobacionComercialLegalCdR } from "../models/aprobacion_comercial_legal_cdr";

export const aprobacionComercialLegalCdRService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<AprobacionComercialLegalCdR | null>> {
    const response = await axiosClient.get<ApiResponse<AprobacionComercialLegalCdR | null>>(
      `/api/AprobacionComercialLegalCdR/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(
    payload: AprobacionComercialLegalCdR,
  ): Promise<ApiResponse<AprobacionComercialLegalCdR>> {
    const response = await axiosClient.post<ApiResponse<AprobacionComercialLegalCdR>>(
      `/api/AprobacionComercialLegalCdR/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/AprobacionComercialLegalCdR/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
