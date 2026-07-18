import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparosGestor } from '../models/corregir_reparos_gestor';
import { corregirReparosGestorService } from '../api/corregirReparosGestorService';

export function useUpsertCorregirReparosGestor() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparosGestor>, unknown, CorregirReparosGestor>({
    mutationFn: (payload) => corregirReparosGestorService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparos_gestor', variables.id_expediente],
      });
    },
  });
}
