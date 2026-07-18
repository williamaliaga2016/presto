import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RegistrarFirmaBancoAcreedorCG } from "../models/registrar_firma_banco_acreedor_cg";

export const registrarFirmaBancoAcreedorCGService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RegistrarFirmaBancoAcreedorCG | null>> {
    const response = await axiosClient.get<
      ApiResponse<RegistrarFirmaBancoAcreedorCG | null>
    >(`/api/RegistrarFirmaBancoAcreedorCG/GetByExpediente/${id_expediente}`);
    return response.data;
  },

  async save(
    payload: RegistrarFirmaBancoAcreedorCG,
  ): Promise<ApiResponse<RegistrarFirmaBancoAcreedorCG>> {
    const response = await axiosClient.post<
      ApiResponse<RegistrarFirmaBancoAcreedorCG>
    >("/api/RegistrarFirmaBancoAcreedorCG/Save", payload);
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RegistrarFirmaBancoAcreedorCG/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
