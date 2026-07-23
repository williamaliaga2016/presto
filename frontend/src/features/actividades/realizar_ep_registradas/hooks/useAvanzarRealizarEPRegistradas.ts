import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarEPRegistradasService } from '../api/realizarEPRegistradasService';

export function useAvanzarRealizarEPRegistradas() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => realizarEPRegistradasService.avanzar(id_expediente),
  });
}
