import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoria } from '../models/gestion_rectificatoria';
import { GestionRectificatoriaService } from '../api/gestionRectificatoriaService';

export function useUpsertGestionRectificatoria() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GestionRectificatoria>, unknown, GestionRectificatoria>({
    mutationFn: (payload) => GestionRectificatoriaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['gestion_rectificatoria', variables.id_expediente],
      });
    },
  });
}
