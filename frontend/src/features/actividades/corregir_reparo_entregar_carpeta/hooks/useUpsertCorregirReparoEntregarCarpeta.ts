import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoEntregarCarpeta } from '../models/corregir_reparo_entregar_carpeta';
import { corregirReparoEntregarCarpetaService } from '../api/corregirReparoEntregarCarpetaService';

export function useUpsertCorregirReparoEntregarCarpeta() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoEntregarCarpeta>, unknown, CorregirReparoEntregarCarpeta>({
    mutationFn: (payload) => corregirReparoEntregarCarpetaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_entregar_carpeta', variables.id_expediente],
      });
    },
  });
}
