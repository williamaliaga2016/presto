import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { memoEscrituraService } from '../api/memoEscrituraService';

export function useAvanzarMemoEscritura() {
  return useMutation<ApiResponse<unknown>, unknown, number>({
    mutationFn: (id_expediente) => memoEscrituraService.avanzar(id_expediente),
  });
}
