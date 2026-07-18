import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecepcionarMatriz } from '../models/recepcionar_matriz';
import { recepcionarMatrizService } from '../api/recepcionarMatrizService';

export function useUpsertRecepcionarMatriz() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RecepcionarMatriz>, unknown, RecepcionarMatriz>({
    mutationFn: (payload) => recepcionarMatrizService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['recepcionar_matriz', variables.id_expediente],
      });
    },
  });
}
