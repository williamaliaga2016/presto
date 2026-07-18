import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AsignarEstudioTitulos } from '../models/asignar_estudio_titulos';
import { asignarEstudioTitulosService } from '../api/asignarEstudioTitulosService';

export function useUpsertAsignarEstudioTitulos() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<AsignarEstudioTitulos>, unknown, AsignarEstudioTitulos>({
    mutationFn: (payload) => asignarEstudioTitulosService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['asignar_estudio_titulos', variables.id_expediente],
      });
    },
  });
}

