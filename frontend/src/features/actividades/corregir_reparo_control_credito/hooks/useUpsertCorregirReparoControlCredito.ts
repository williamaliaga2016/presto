import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoControlCredito } from '../models/corregirReparoControlCredito';
import { corregirReparoControlCreditoService } from '../api/corregirReparoControlCreditoService';

export function useUpsertCorregirReparoControlCredito() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoControlCredito>, unknown, CorregirReparoControlCredito>({
    mutationFn: (payload) => corregirReparoControlCreditoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_fabrica', variables.id_expediente],
      });
    },
  });
}
