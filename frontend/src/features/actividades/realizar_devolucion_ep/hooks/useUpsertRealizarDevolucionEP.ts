import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarDevolucionEP } from '../models/realizar_devolucion_ep';
import { realizarDevolucionEPService } from '../api/realizarDevolucionEPService';

export function useUpsertRealizarDevolucionEP() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<RealizarDevolucionEP>, unknown, RealizarDevolucionEP>({
    mutationFn: (payload) => realizarDevolucionEPService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['realizar_devolucion_ep', variables.id_expediente] });
    },
  });
}
