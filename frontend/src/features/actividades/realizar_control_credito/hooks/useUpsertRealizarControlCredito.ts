import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarControlCredito } from '../models/realizar_control_credito';
import { realizarControlCreditoService } from '../api/realizarControlCreditoService';

export function useUpsertRealizarControlCredito() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RealizarControlCredito>, unknown, RealizarControlCredito>({
    mutationFn: (payload) => realizarControlCreditoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['realizar_control_credito', variables.id_expediente],
      });
    },
  });
}
