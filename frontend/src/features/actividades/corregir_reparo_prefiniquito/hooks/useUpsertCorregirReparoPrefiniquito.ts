import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoPrefiniquito } from '../models/corregir_reparo_prefiniquito';
import { corregirReparoPrefiniquitoService } from '../api/corregirReparoPrefiniquitoService';

export function useUpsertCorregirReparoPrefiniquito() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoPrefiniquito>, unknown, CorregirReparoPrefiniquito>({
    mutationFn: (payload) => corregirReparoPrefiniquitoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_prefiniquito', variables.id_expediente],
      });
    },
  });
}
