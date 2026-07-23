import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';
import { revisarEpAbogadoService } from '../api/revisarEpAbogadoService';

export function useUpsertRevisarEp() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RevisarEpAbogado>, unknown, RevisarEpAbogado>({
    mutationFn: (payload) => revisarEpAbogadoService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_ep_abogado', variables.id_expediente],
      });
    },
  });
}
