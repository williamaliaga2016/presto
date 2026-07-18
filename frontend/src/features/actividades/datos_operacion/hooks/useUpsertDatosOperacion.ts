import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';
import type { DatosOperacion } from '../models/datos_operacion';

export function useUpsertDatosOperacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<DatosOperacion>, unknown, DatosOperacion>({
    mutationFn: (payload) => datosOperacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['datos_operacion_5_4', variables.id_expediente],
      });
    },
  });
}
