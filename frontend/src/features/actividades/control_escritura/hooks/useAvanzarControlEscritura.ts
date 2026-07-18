import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { controlEscrituraService } from '../api/controlEscrituraService';

export function useAvanzarControlEscritura() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => controlEscrituraService.avanzar(id_expediente),
  });
}
