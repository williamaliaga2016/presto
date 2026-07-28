import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisionMarcacionCobertura } from '../models/revision_marcacion_cobertura';
import { revisarMarcacionCoberturaService } from '../api/revisarMarcacionCoberturaService';

export function useUpsertRevisionMarcacionCobertura() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RevisionMarcacionCobertura>, unknown, RevisionMarcacionCobertura>({
    mutationFn: (payload) => revisarMarcacionCoberturaService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revision_marcacion_cobertura', variables.id_expediente],
      });
    },
  });
}
