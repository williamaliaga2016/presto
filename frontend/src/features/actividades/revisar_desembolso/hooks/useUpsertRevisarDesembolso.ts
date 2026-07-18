import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarDesembolso } from '../models/revisar_desembolso';
import { revisarDesembolsoService } from '../api/revisarDesembolsoService';

export function useUpsertRevisarDesembolso() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<RevisarDesembolso>, unknown, RevisarDesembolso>({
    mutationFn: (payload) => revisarDesembolsoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_desembolso', variables.id_expediente],
      });
    },
  });
}