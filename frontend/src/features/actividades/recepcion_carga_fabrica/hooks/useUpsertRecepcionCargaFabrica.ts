import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecepcionCargaFabrica } from '../models/recepcion_carga_fabrica';
import { recepcionCargaFabricaService } from '../api/recepcionCargaFabricaService';

export function useUpsertRecepcionCargaFabrica() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RecepcionCargaFabrica>, unknown, RecepcionCargaFabrica>({
    mutationFn: (payload) => recepcionCargaFabricaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['recepcion_carga_fabrica', variables.id_expediente],
      });
    },
  });
}
