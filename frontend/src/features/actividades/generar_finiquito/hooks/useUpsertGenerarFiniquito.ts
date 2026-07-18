import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarFiniquito } from '../models/generar_finiquito';
import { generarFiniquitoService } from '../api/generarFiniquitoService';

export function useUpsertGenerarFiniquito() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GenerarFiniquito>, unknown, GenerarFiniquito>({
    mutationFn: (payload) => generarFiniquitoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['generar_finiquito', variables.id_expediente],
      });
    },
  });
}
