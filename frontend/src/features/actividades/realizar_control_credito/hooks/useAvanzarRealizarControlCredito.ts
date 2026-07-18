import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarControlCreditoService } from '../api/realizarControlCreditoService';

export function useAvanzarRealizarControlCredito() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => realizarControlCreditoService.avanzar(id_expediente),
  });
}
