import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoLiquidacion } from '../models/corregir_reparo_liquidacion';
import { corregirReparoLiquidacionService } from '../api/corregirReparoLiquidacionService';

export function useUpsertCorregirReparoLiquidacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoLiquidacion>, unknown, CorregirReparoLiquidacion>({
    mutationFn: (payload) => corregirReparoLiquidacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_liquidacion', variables.id_expediente],
      });
    },
  });
}
