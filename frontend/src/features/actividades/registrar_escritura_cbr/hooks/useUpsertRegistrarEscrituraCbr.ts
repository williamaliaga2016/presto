import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarEscrituraCbr } from '../models/registrar_escritura_cbr';
import { registrarEscrituraCbrService } from '../api/registrarEscrituraCbrService';

export function useUpsertRegistrarEscrituraCbr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RegistrarEscrituraCbr>, unknown, RegistrarEscrituraCbr>({
    mutationFn: (payload) => registrarEscrituraCbrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['registrar_escritura_cbr', variables.id_expediente],
      });
    },
  });
}
