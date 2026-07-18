import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarRecursosPagosCbr } from '../models/generar_recursos_pagos_cbr';
import { generarRecursosPagosCbrService } from '../api/generarRecursosPagosCbrService';

export function useUpsertGenerarRecursosPagosCbr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GenerarRecursosPagosCbr>, unknown, GenerarRecursosPagosCbr>({
    mutationFn: (payload) => generarRecursosPagosCbrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['generar_recursos_pagos_cbr', variables.id_expediente],
      });
    },
  });
}
