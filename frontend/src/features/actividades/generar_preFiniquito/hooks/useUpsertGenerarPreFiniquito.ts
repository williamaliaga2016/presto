import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarPreFiniquito } from '../models/generar_preFiniquito';
import { generarPreFiniquitoService } from '../api/generarPreFiniquitoService';

export function useUpsertGenerarPreFiniquito() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<GenerarPreFiniquito>, unknown, GenerarPreFiniquito>({
    mutationFn: (payload) => generarPreFiniquitoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['generar_preFiniquito', variables.id_expediente],
      });
    },
  });
}