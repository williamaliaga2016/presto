import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarDevolucionEPService } from '../api/realizarDevolucionEPService';

export function useAvanzarRealizarDevolucionEP() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => realizarDevolucionEPService.avanzar(id_expediente),
  });
}
