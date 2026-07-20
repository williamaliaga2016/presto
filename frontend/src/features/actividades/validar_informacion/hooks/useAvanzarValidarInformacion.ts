import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validarInformacionService } from '../api/validarInformacionService';
import type { AvanzarValidarInformacionResponse } from
  '../models/avanzar_validar_informacion';

/**
 * Ejecuta el avance de Validar Informacion y expone el acceso temporal generado cuando aplica.
 *
 * @returns Mutacion para avanzar el expediente actual.
 */
export function useAvanzarValidarInformacion() {
  return useMutation<ApiResponse<AvanzarValidarInformacionResponse>, Error, number>({
    mutationFn: (id_expediente) =>
      validarInformacionService.avanzar(id_expediente),
  });
}
