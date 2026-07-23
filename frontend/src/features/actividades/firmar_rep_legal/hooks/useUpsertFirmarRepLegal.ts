import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useUpsertFirmarRepLegal() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<FirmarRepLegal>, unknown, FirmarRepLegal>({
    mutationFn: (payload) => firmarRepLegalService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['firmar_rep_legal', variables.id_expediente],
      });
    },
  });
}
