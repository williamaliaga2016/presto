import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { RevisarDatosOperacion } from '../models/revisar_datos_operacion';

export function useUpsertRevisarDatosOperacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RevisarDatosOperacion>, unknown, RevisarDatosOperacion>({
    mutationFn: (payload) => revisarDatosOperacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_datos_operacion', variables.id_expediente],
      });
      queryClient.invalidateQueries({
        queryKey: ['revisar_datos_operacion_banco', variables.id_expediente],
      });
    },
  });
}
