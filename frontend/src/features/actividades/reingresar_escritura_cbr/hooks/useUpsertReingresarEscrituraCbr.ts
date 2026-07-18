import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ReingresarEscrituraCbr } from '../models/reingresar_escritura_cbr';
import { reingresarEscrituraCbrService } from '../api/reingresarEscrituraCbrService';

export function useUpsertReingresarEscrituraCbr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ReingresarEscrituraCbr>, unknown, ReingresarEscrituraCbr>({
    mutationFn: (payload) => reingresarEscrituraCbrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['reingresar_escritura_cbr', variables.id_expediente],
      });
    },
  });
}
