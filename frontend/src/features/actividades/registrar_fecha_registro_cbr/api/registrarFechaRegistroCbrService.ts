import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarFechaRegistroCbr } from '../models/registrar_fecha_registro_cbr';

export const registrarFechaRegistroCbrService = {
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RegistrarFechaRegistroCbr | null>> {
    const response = await axiosClient.get<ApiResponse<RegistrarFechaRegistroCbr | null>>(
      `/api/RegistrarFechaRegistroCbr/GetByExpediente/${id_expediente}`,
    );

    return response.data;
  },

  async save(
    payload: RegistrarFechaRegistroCbr,
  ): Promise<ApiResponse<RegistrarFechaRegistroCbr>> {
    const response = await axiosClient.post<ApiResponse<RegistrarFechaRegistroCbr>>(
      `/api/RegistrarFechaRegistroCbr/Save`,
      payload,
    );

    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<boolean>> {
    const response = await axiosClient.get<ApiResponse<boolean>>(
      `/api/RegistrarFechaRegistroCbr/Avanzar/${id_expediente}`,
    );

    return response.data;
  },
};
