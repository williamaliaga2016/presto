import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  MemoEscrituraRequest,
  MemoEscrituraVersion,
} from '../models/memo_escritura';
import { memoEscrituraService } from '../api/memoEscrituraService';

export function useGenerarPdfMemoEscritura() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<MemoEscrituraVersion>,
    unknown,
    MemoEscrituraRequest
  >({
    mutationFn: (payload) => memoEscrituraService.generarPdf(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['memo_escritura_versiones', variables.id_expediente],
      });
    },
  });
}
