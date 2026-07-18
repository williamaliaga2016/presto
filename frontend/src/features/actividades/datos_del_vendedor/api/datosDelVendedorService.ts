import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '@/features/actividades/datos_operacion/api/datosOperacionService';
import type { DatosOperacion } from '@/features/actividades/datos_operacion/models/datos_operacion';

const baseUrl = '/api/DatosDelVendedor';

export const datosDelVendedorService = {
  async getFullByExpediente(id_expediente: number): Promise<ApiResponse<DatosOperacion | null>> {
    return datosOperacionService.getFullByExpediente(id_expediente);
  },

  async saveVendedores(payload: DatosOperacion): Promise<ApiResponse<DatosOperacion>> {
    const response = await axiosClient.post<ApiResponse<DatosOperacion>>(
      `${baseUrl}/Save`,
      payload,
    );
    return response.data;
  },

  async avanzar(id_expediente: number): Promise<ApiResponse<unknown>> {
    const response = await axiosClient.get<ApiResponse<unknown>>(
      `${baseUrl}/Avanzar/${id_expediente}`,
    );
    return response.data;
  },
};
