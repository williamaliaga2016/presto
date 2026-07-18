import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AsignarEscritura } from '../models/asignar_escritura';
import { asignarEscrituraService } from '../api/asignarEscrituraService';

export function useUpsertAsignarEscritura() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<AsignarEscritura>, unknown, AsignarEscritura>({
    mutationFn: (payload) => asignarEscrituraService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['asignar_escritura', variables.id_expediente],
      });
    },
  });
}
