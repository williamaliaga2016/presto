import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ExcepcionDesembolso } from '../models/excepcion_desembolso';
import { excepcionDesembolsoService } from '../api/excepcionDesembolsoService';

export function useUpsertExcepcionDesembolso() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ExcepcionDesembolso>, unknown, ExcepcionDesembolso>({
    mutationFn: (payload) => excepcionDesembolsoService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['excepcion_desembolso', variables.id_expediente],
      });
    },
  });
}
