import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlEscritura } from '../models/control_escritura';
import { controlEscrituraService } from '../api/controlEscrituraService';

export function useUpsertControlEscritura() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<ControlEscritura>, unknown, ControlEscritura>({
    mutationFn: (payload) => controlEscrituraService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['control_escritura', variables.id_expediente],
      });
    },
  });
}
