import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarEntregaEpFirmada } from '../models/realizar_entrega_ep_firmada';
import { realizarEntregaEpFirmadaService } from '../api/realizarEntregaEpFirmadaService';

export function useUpsertRealizarEntregaEpFirmada() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RealizarEntregaEpFirmada>, unknown, RealizarEntregaEpFirmada>({
    mutationFn: (payload) => realizarEntregaEpFirmadaService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['realizar_entrega_ep_firmada', variables.id_expediente],
      });
    },
  });
}
