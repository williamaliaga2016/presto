import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ExcepcionDesembolso,
  ExcepcionDesembolsoResponse,
} from '../models/excepcion_desembolso';

const PATH_URL = '/api/excepcion-desembolso';

export const excepcionDesembolsoService = {
  /**
   * GET /api/excepcion-desembolso/{idExpediente}
   * Retorna formulario + datos heredados dinámicos + origen del caso.
   */
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<ExcepcionDesembolsoResponse | null>> {
    const response = await axiosClient.get<
      ApiResponse<ExcepcionDesembolsoResponse | null>
    >(`${PATH_URL}/GetByIdExpediente/${id_expediente}`);
    return response.data;
  },

  /**
   * POST /api/excepcion-desembolso/guardar
   * Upsert sin validar campos obligatorios.
   */
  async guardar(
    payload: ExcepcionDesembolso,
  ): Promise<ApiResponse<ExcepcionDesembolso>> {
    const response = await axiosClient.post<ApiResponse<ExcepcionDesembolso>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  /**
   * POST /api/excepcion-desembolso/{idExpediente}/avanzar
   * Valida obligatorios, enruta y registra bitácora.
   */
  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<{ actividad_destino: string } | null>> {
    const response = await axiosClient.post<
      ApiResponse<{ actividad_destino: string } | null>
    >(`${PATH_URL}/avanzar/${id_expediente}`);
    return response.data;
  },
};
