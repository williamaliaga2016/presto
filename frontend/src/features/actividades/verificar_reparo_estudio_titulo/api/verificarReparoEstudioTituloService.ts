import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarReparoEstudioTitulo } from '../models/verificar_reparo_estudio_titulo';

export const verificarReparoEstudioTituloService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<VerificarReparoEstudioTitulo | null>> {
    const response = await axiosClient.get<ApiResponse<VerificarReparoEstudioTitulo | null>>(
      `/api/VerificarReparoEstudioTitulo/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: VerificarReparoEstudioTitulo,
  ): Promise<ApiResponse<VerificarReparoEstudioTitulo>> {
    const response = await axiosClient.post<ApiResponse<VerificarReparoEstudioTitulo>>(
      `/api/VerificarReparoEstudioTitulo/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/VerificarReparoEstudioTitulo/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
