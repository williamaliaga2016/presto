import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarCopiasEscrituras } from '../models/revisar_copias_escritura';
import { revisarCopiasEscriturasService } from '../api/revisarCopiasEscriturasService';

export function useUpsertRevisarCopiasEscrituras() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RevisarCopiasEscrituras>, unknown, RevisarCopiasEscrituras>({
    mutationFn: (payload) => revisarCopiasEscriturasService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_copias_escritura', variables.id_expediente],
      });
    },
  });
}
