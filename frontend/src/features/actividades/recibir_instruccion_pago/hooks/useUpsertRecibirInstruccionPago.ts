import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecibirInstruccionPago } from '../models/recibir_instruccion_pago';
import { recibirInstruccionPagoService } from '../api/recibirInstruccionPagoService';

export function useUpsertRecibirInstruccionPago() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RecibirInstruccionPago>, unknown, RecibirInstruccionPago>({
    mutationFn: (payload) => recibirInstruccionPagoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['recibir_instruccion_pago', variables.id_expediente],
      });
    },
  });
}
