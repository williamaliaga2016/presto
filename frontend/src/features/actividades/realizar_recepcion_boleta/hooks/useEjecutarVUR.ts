import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarRecepcionBoleta } from '../models/realizar_recepcion_boleta';
import { realizarRecepcionBoletaService } from '../api/realizarRecepcionBoletaService';

export function useEjecutarVUR(id_expediente: number) {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RealizarRecepcionBoleta>, unknown, void>({
    mutationFn: () => realizarRecepcionBoletaService.ejecutarVUR(id_expediente),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ['realizar_recepcion_boleta', id_expediente],
      });
    },
  });
}
