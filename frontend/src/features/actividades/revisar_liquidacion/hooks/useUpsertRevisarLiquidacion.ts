import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarLiquidacion } from '../models/revisar_liquidacion';
import { revisarLiquidacionService } from '../api/revisarLiquidacionService';

export function useUpsertRevisarLiquidacion() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<RevisarLiquidacion>, unknown, RevisarLiquidacion>({
    mutationFn: (payload) => revisarLiquidacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_liquidacion', variables.id_expediente],
      });
    },
  });
}
