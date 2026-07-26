import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  VoboGerenciaCoh,
  VoboGerenciaCohResponse,
} from '../models/vobo_gerencia_coh';

const PATH_URL = '/api/VoboGerenciaCoh';

export const realizarVoboGerenciaCohService = {
  /**
   * GET /api/VoboGerenciaCoh/GetByExpediente/{idExpediente}
   * Retorna formulario editable + datos heredados de Excepción Desembolso.
   */
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<VoboGerenciaCohResponse | null>> {
    const response = await axiosClient.get<
      ApiResponse<VoboGerenciaCohResponse | null>
    >(`${PATH_URL}/GetByExpediente/${id_expediente}`);
    return response.data;
  },

  /**
   * POST /api/VoboGerenciaCoh/Save
   * Upsert sin validar campos obligatorios.
   */
  async guardar(
    payload: VoboGerenciaCoh,
  ): Promise<ApiResponse<VoboGerenciaCoh>> {
    const response = await axiosClient.post<ApiResponse<VoboGerenciaCoh>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  /**
   * GET /api/VoboGerenciaCoh/Avanzar/{idExpediente}
   * Valida campos obligatorios, ejecuta enrutamiento y registra bitácora.
   */
  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<{ actividad_destino: string } | null>> {
    const response = await axiosClient.get<
      ApiResponse<{ actividad_destino: string } | null>
    >(`${PATH_URL}/Avanzar/${id_expediente}`);
    return response.data;
  },
};
