import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValorizarCbr } from '../models/valorizar_cbr';
import { valorizarCbrService } from '../api/valorizarCbrService';

export function useUpsertValorizarCbr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ValorizarCbr>, unknown, ValorizarCbr>({
    mutationFn: (payload) => valorizarCbrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['valorizar_cbr', variables.id_expediente],
      });
    },
  });
}
