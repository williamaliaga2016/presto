import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarRecepcionBoleta } from '../models/realizar_recepcion_boleta';
import { realizarRecepcionBoletaService } from '../api/realizarRecepcionBoletaService';

export function useUpsertRealizarRecepcionBoleta() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RealizarRecepcionBoleta>, unknown, RealizarRecepcionBoleta>({
    mutationFn: (payload) => realizarRecepcionBoletaService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['realizar_recepcion_boleta', variables.id_expediente],
      });
    },
  });
}
