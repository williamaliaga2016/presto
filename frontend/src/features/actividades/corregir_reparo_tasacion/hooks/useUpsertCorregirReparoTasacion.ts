import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoTasacion } from '../models/corregir_reparo_tasacion';
import { corregirReparoTasacionService } from '../api/corregirReparoTasacionService';

export function useUpsertCorregirReparoTasacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoTasacion>, unknown, CorregirReparoTasacion>({
    mutationFn: (payload) => corregirReparoTasacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_tasacion', variables.id_expediente],
      });
    },
  });
}
