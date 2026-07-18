import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { VerificarReparoDatosOperacion } from "../models/verificar_reparo_datos_operacion";

export const verificarReparoIngresarDatosOperacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<VerificarReparoDatosOperacion | null>> {
    const response = await axiosClient.get<ApiResponse<VerificarReparoDatosOperacion | null>>(
      `/api/VerificarReparoIngresarDatosOperacion/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: VerificarReparoDatosOperacion,
  ): Promise<ApiResponse<VerificarReparoDatosOperacion>> {
    const response = await axiosClient.post<ApiResponse<VerificarReparoDatosOperacion>>(
      `/api/VerificarReparoIngresarDatosOperacion/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.get<ApiResponse<unknown>>(
      `/api/VerificarReparoIngresarDatosOperacion/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
