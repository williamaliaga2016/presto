import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparoControlCreditoService } from '../api/corregirReparoControlCreditoService';

export function useAvanzarCorregirReparoControlCredito() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoControlCreditoService.avanzar(id_expediente),
  });
}
