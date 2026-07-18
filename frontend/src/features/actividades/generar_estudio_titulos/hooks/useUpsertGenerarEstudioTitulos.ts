import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarEstudioTitulos } from '../models/generar_estudio_titulos';
import { generarEstudioTitulosService } from '../api/generarEstudioTitulosService';

export function useUpsertGenerarEstudioTitulos() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<GenerarEstudioTitulos>, unknown, GenerarEstudioTitulos>({
    mutationFn: (payload) => generarEstudioTitulosService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['generar_estudio_titulos', variables.id_expediente],
      });
    },
  });
}