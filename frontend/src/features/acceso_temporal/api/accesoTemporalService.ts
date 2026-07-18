import { axiosClient } from '@/core/api/axiosClient';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  AccesoTemporalValidarRequest,
  AccesoTemporalValidarResponse,
} from '../models/accesoTemporal';

const baseUrl = '/api/acceso-temporal';

interface ProblemDetailsResponse {
  title?: string;
  status?: number;
  errors?: Record<string, string[]>;
}

function isApiResponse<T>(value: unknown): value is ApiResponse<T> {
  return (
    typeof value === 'object' &&
    value !== null &&
    'status' in value &&
    typeof (value as ApiResponse<T>).status === 'boolean'
  );
}

/**
 * Convierte errores de validacion ASP.NET a la forma de respuesta usada por la pantalla publica.
 *
 * @param problem - Respuesta problem+json retornada antes de entrar al controller.
 * @returns Respuesta controlada para detener el estado de validacion.
 */
function problemToApiResponse(
  problem: ProblemDetailsResponse,
): ApiResponse<AccesoTemporalValidarResponse | null> {
  const tokenErrors = problem.errors?.token ?? problem.errors?.Token ?? [];

  return {
    status: false,
    detail: null,
    message: tokenErrors[0] ?? problem.title ?? 'Este link ya no se encuentra disponible',
  };
}

export const accesoTemporalService = {
  /**
   * Valida y consume el token temporal; si es valido, backend retorna un JWT temporal.
   *
   * @param payload - Token UUID recibido desde la ruta publica.
   * @returns Respuesta API con JWT temporal, expediente, actividad y ruta de destino.
   */
  async validar(
    payload: AccesoTemporalValidarRequest,
  ): Promise<ApiResponse<AccesoTemporalValidarResponse | null>> {
    const response = await axiosClient.post<
      ApiResponse<AccesoTemporalValidarResponse> | ProblemDetailsResponse
    >(
      `${baseUrl}/validar`,
      payload,
      {
        validateStatus: (status) => status < 500,
      },
    );

    if (isApiResponse<AccesoTemporalValidarResponse>(response.data)) {
      return response.data;
    }

    return problemToApiResponse(response.data);
  },
};
