import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { EvaluarReparoAutomatico, Tasacion } from "../models/registrar_tasacion";
import type { TasacionDetalle } from "../models/tasacion_detalle";

export const registrarTasacionService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<Tasacion | null>> {
    const response = await axiosClient.get<ApiResponse<Tasacion | null>>(
      `/api/Tasacion/GetByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async getDetallesByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<TasacionDetalle[]>> {
    const response = await axiosClient.get<ApiResponse<TasacionDetalle[]>>(
      `/api/Tasacion/GetDetallesByExpediente/${id_expediente}`,
    );
    return response.data;
  },

  async save(payload: Tasacion): Promise<ApiResponse<Tasacion>> {
    const response = await axiosClient.post<ApiResponse<Tasacion>>(
      `/api/Tasacion/Save`,
      payload,
    );
    return response.data;
  },

  async eliminarDetalle(
    id_tasacion_detalle: number,
  ): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.delete<ApiResponse<boolean>>(
      `/api/Tasacion/EliminarDetalle/${id_tasacion_detalle}`,
    );
    return response.data;
  },

  async evaluarReparoAutomatico(
    id_expediente: number,
  ): Promise<ApiResponse<EvaluarReparoAutomatico>> {
    const response = await axiosClient.get<ApiResponse<EvaluarReparoAutomatico>>(
      `/api/Tasacion/EvaluarReparoAutomatico/${id_expediente}`,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/Tasacion/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
