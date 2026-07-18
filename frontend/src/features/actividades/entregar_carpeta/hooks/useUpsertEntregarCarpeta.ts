import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { EntregarCarpeta } from '../models/entregar_carpeta';
import { entregarCarpetaService } from '../api/entregarCarpetaService';

export function useUpsertEntregarCarpeta() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<EntregarCarpeta>, unknown, EntregarCarpeta>({
    mutationFn: (payload) => entregarCarpetaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['entregar_carpeta', variables.id_expediente],
      });
    },
  });
}
