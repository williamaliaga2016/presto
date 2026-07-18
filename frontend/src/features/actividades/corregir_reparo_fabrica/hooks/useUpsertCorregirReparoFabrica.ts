import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoFabrica } from '../models/corregir_reparo_fabrica';
import { corregirReparoFabricaService } from '../api/corregirReparoFabricaService';

export function useUpsertCorregirReparoFabrica() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoFabrica>, unknown, CorregirReparoFabrica>({
    mutationFn: (payload) => corregirReparoFabricaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_fabrica', variables.id_expediente],
      });
    },
  });
}
