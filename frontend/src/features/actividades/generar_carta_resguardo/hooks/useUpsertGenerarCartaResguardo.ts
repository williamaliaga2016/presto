import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarCartaResguardo } from '../models/generar_carta_resguardo';
import { generarCartaResguardoService } from '../api/generarCartaResguardoService';

export function useUpsertGenerarCartaResguardo() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GenerarCartaResguardo>, unknown, GenerarCartaResguardo>({
    mutationFn: (payload) => generarCartaResguardoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['generar_carta_resguardo', variables.id_expediente],
      });
    },
  });
}
