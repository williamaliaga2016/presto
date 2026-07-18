import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarInscripcionCbr } from '../models/revisar_inscripcion_cbr';
import { revisarInscripcionCbrService } from '../api/revisarInscripcionCbrService';

export function useUpsertRevisarInscripcionCbr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RevisarInscripcionCbr>, unknown, RevisarInscripcionCbr>({
    mutationFn: (payload) => revisarInscripcionCbrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_inscripcion_cbr', variables.id_expediente],
      });
    },
  });
}
