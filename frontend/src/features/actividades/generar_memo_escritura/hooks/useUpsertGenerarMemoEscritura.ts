import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarMemoEscritura } from '../models/generar_memo_escritura';
import { memoEscrituraService } from '../api/memoEscrituraService';

export function useUpsertGenerarMemoEscritura() {
  const queryClient = useQueryClient();
  return useMutation<
    ApiResponse<GenerarMemoEscritura>,
    unknown,
    GenerarMemoEscritura
  >({
    mutationFn: (payload) => memoEscrituraService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['generar_memo_escritura', variables.id_expediente],
      });
    },
  });
}
