import { axiosClient } from "@/core/api/axiosClient";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type {
  CargarDocumentosCliente,
  CargarDocumentosClienteInfo,
} from "../models/cargar_documentos_cliente";

const baseUrl = "/api/cargar-documentos-cliente";

export const cargarDocumentosClienteService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<CargarDocumentosCliente | null>> {
    const response = await axiosClient.get<
      ApiResponse<CargarDocumentosCliente | null>
    >(`${baseUrl}/GetByExpediente/${id_expediente}`);

    return response.data;
  },

  async getInfoCliente(
    id_expediente: number,
  ): Promise<ApiResponse<CargarDocumentosClienteInfo>> {
    const response = await axiosClient.get<
      ApiResponse<CargarDocumentosClienteInfo>
    >(`${baseUrl}/info-cliente/${id_expediente}`);

    return response.data;
  },

  async guardar(
    payload: CargarDocumentosCliente,
  ): Promise<ApiResponse<CargarDocumentosCliente>> {
    const response = await axiosClient.post<
      ApiResponse<CargarDocumentosCliente>
    >(`${baseUrl}/Save`, payload);

    return response.data;
  },

  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.post<ApiResponse<unknown>>(
      `${baseUrl}/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
