import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VisarOperacion } from '../models/visar_operacion';
import { visarOperacionService } from '../api/visarOperacionService';

export function useUpsertVisarOperacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<VisarOperacion>, unknown, VisarOperacion>({
    mutationFn: (payload) => visarOperacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['visar_operacion', variables.id_expediente],
      });
    },
  });
}
