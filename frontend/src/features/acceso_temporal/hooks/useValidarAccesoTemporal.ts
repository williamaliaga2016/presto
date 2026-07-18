import { useMutation } from '@tanstack/react-query';
import { accesoTemporalService } from '../api/accesoTemporalService';
import type {
  AccesoTemporalValidarRequest,
  AccesoTemporalValidarResponse,
} from '../models/accesoTemporal';
import type { ApiResponse } from '@/core/api/models/ApiResponse';

/**
 * Mutacion publica que valida un token temporal sin requerir una sesion previa.
 *
 * @returns Mutacion de TanStack Query para validar y consumir el token temporal.
 */
export function useValidarAccesoTemporal() {
  return useMutation<
    ApiResponse<AccesoTemporalValidarResponse | null>,
    Error,
    AccesoTemporalValidarRequest
  >({
    mutationFn: accesoTemporalService.validar,
  });
}
