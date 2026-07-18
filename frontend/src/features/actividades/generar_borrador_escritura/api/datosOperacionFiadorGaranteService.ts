// src/features/generar_borrador_escritura/api/generarBorradorEscrituraService.ts

import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FiadorGarante } from '../models/generarBorradorEscritura';

const baseUrl = 'api/DatosOperacion';

export const datosOperacionFiadorGaranteService = {
  /**
   * Obtiene los fiadores/garantes por expediente
   */
  async getFiadoresGarantesByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<FiadorGarante[] | null>> {
    const response = await axiosClient.get<ApiResponse<FiadorGarante[] | null>>(
      `${baseUrl}/GetFiadoresGarantesByExpediente/${id_expediente}`,
    );
    return response.data;
  },

};