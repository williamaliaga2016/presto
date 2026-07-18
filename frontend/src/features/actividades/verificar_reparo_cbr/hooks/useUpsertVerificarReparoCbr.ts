import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarReparoCbr } from '../models/verificar_reparo_cbr';
import { verificarReparoCbrService } from '../api/verificarReparoCbrService';

export function useUpsertVerificarReparoCbr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<VerificarReparoCbr>, unknown, VerificarReparoCbr>({
    mutationFn: (payload) => verificarReparoCbrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['verificar_reparo_cbr', variables.id_expediente],
      });
    },
  });
}
