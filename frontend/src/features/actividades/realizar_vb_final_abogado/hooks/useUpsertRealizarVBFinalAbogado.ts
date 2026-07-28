import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarVBFinalAbogado } from '../models/realizar_vb_final_abogado';
import { realizarVBFinalAbogadoService } from '../api/realizarVBFinalAbogadoService';

export function useUpsertRealizarVBFinalAbogado() {
  const queryClient = useQueryClient();
  return useMutation<ApiResponse<RealizarVBFinalAbogado>, unknown, RealizarVBFinalAbogado>({
    mutationFn: (payload) => realizarVBFinalAbogadoService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['realizar_vb_final_abogado', variables.id_expediente] });
    },
  });
}
