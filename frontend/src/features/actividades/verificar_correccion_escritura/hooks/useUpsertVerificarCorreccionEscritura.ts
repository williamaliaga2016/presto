import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarCorreccionEscritura } from '../models/verificar_correccion_escritura';
import { verificarCorreccionEscrituraService } from '../api/verificarCorreccionEscrituraService';

export function useUpsertVerificarCorreccionEscritura() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<VerificarCorreccionEscritura>, unknown, VerificarCorreccionEscritura>({
    mutationFn: (payload) => verificarCorreccionEscrituraService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['verificar_correccion_escritura', variables.id_expediente],
      });
    },
  });
}