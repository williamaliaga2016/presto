import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarEPRegistradas } from '../models/realizar_ep_registradas';
import { realizarEPRegistradasService } from '../api/realizarEPRegistradasService';

export function useUpsertRealizarEPRegistradas() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<RealizarEPRegistradas>, unknown, RealizarEPRegistradas>({
    mutationFn: (payload) => realizarEPRegistradasService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['realizar_ep_registradas', variables.id_expediente] });
    },
  });
}
