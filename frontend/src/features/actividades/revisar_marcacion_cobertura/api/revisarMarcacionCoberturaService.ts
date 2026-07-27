import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  RevisionMarcacionCobertura,
  RevisionMarcacionCoberturaResponse,
} from '../models/revision_marcacion_cobertura';
import type { ControlesRevisionMarcacionCobertura } from '../models/controles';

const PATH_URL = '/api/RevisionMarcacionCobertura';

export const revisarMarcacionCoberturaService = {
  /**
   * GET /api/RevisionMarcacionCobertura/GetByExpediente/{idExpediente}
   * Retorna formulario editable + datos heredados del encabezado (CA02).
   */
  async getByExpediente(
    id_expediente: number,
  ): Promise<ApiResponse<RevisionMarcacionCoberturaResponse | null>> {
    const response = await axiosClient.get<
      ApiResponse<RevisionMarcacionCoberturaResponse | null>
    >(`${PATH_URL}/GetByExpediente/${id_expediente}`);
    return response.data;
  },

  /**
   * POST /api/RevisionMarcacionCobertura/Save
   * Upsert sin validar campos obligatorios.
   */
  async guardar(
    payload: RevisionMarcacionCobertura,
  ): Promise<ApiResponse<RevisionMarcacionCobertura>> {
    const response = await axiosClient.post<ApiResponse<RevisionMarcacionCobertura>>(
      `${PATH_URL}/Save`,
      payload,
    );
    return response.data;
  },

  /**
   * GET /api/RevisionMarcacionCobertura/Avanzar/{idExpediente}
   * Valida campos obligatorios y correo (CA08), envía notificación (CA05),
   * ejecuta enrutamiento y registra bitácora.
   */
  async avanzar(
    id_expediente: number,
  ): Promise<ApiResponse<{ actividad_destino: string } | null>> {
    const response = await axiosClient.get<
      ApiResponse<{ actividad_destino: string } | null>
    >(`${PATH_URL}/Avanzar/${id_expediente}`);
    return response.data;
  },

  /**
   * GET /api/RevisionMarcacionCobertura/controles
   * Catálogos para los dropdowns de Tipo de Documento, Tipo de Vivienda y Estado Proceso.
   */
  async getControles(): Promise<ApiResponse<ControlesRevisionMarcacionCobertura>> {
    const response = await axiosClient.get<ApiResponse<ControlesRevisionMarcacionCobertura>>(
      `${PATH_URL}/controles`,
    );
    return response.data;
  },
};
