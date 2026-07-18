import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CierreCopiasNotaria } from '../models/cierre_copias_notaria';
import { cierreCopiasNotariaService } from '../api/cierreCopiasNotariaService';

export function useUpsertCierreCopiasNotaria() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CierreCopiasNotaria>, unknown, CierreCopiasNotaria>({
    mutationFn: (payload) => cierreCopiasNotariaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['cierre_copias_notaria', variables.id_expediente],
      });
    },
  });
}
