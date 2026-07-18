import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoInstPago } from '../models/corregir_reparo_inst_pago';
import { corregirReparoInstPagoService } from '../api/corregirReparoInstPagoService';

export function useUpsertCorregirReparoInstPago() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoInstPago>, unknown, CorregirReparoInstPago>({
    mutationFn: (payload) => corregirReparoInstPagoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_inst_pago', variables.id_expediente],
      });
    },
  });
}
