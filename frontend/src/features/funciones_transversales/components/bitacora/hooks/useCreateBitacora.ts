import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { Bitacora } from '../models/bitacora';
import { bitacoraService } from '../api/bitacoraService';

export function useCreateBitacora() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<Bitacora>, unknown, Bitacora>({
    mutationFn: (payload) => bitacoraService.create(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['bitacora', variables.id_expediente],
      });
    },
  });
}
