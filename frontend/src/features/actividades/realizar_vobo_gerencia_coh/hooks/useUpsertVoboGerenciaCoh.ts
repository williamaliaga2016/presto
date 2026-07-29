import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VoboGerenciaCoh } from '../models/vobo_gerencia_coh';
import { realizarVoboGerenciaCohService } from '../api/realizarVoboGerenciaCohService';

export function useUpsertVoboGerenciaCoh() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<VoboGerenciaCoh>, unknown, VoboGerenciaCoh>({
    mutationFn: (payload) => realizarVoboGerenciaCohService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['vobo_gerencia_coh', variables.id_expediente],
      });
    },
  });
}
