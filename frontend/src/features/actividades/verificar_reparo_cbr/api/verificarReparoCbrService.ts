import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarReparoCbr } from '../models/verificar_reparo_cbr';
import { ControlesVerificarReparoCbr } from '../models/catalogo';

export const verificarReparoCbrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<VerificarReparoCbr | null>> {
    const response = await axiosClient.get<ApiResponse<VerificarReparoCbr | null>>(
      `/api/VerificarReparoCbr/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: VerificarReparoCbr,
  ): Promise<ApiResponse<VerificarReparoCbr>> {
    const response = await axiosClient.post<ApiResponse<VerificarReparoCbr>>(
      `/api/VerificarReparoCbr/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/VerificarReparoCbr/Avanzar/${id_expediente}`,
    );

    return response.data;
  },

  async getControlesVerificarReparoCbr(): Promise<ApiResponse<ControlesVerificarReparoCbr>> {
    const response = await axiosClient.get<ApiResponse<ControlesVerificarReparoCbr>>(
      `/api/VerificarReparoCbr/getControlesVerificarReparoCbr`,
    );

    return response.data;
  },
};
