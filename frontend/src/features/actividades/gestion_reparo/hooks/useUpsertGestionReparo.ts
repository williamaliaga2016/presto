import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionReparo } from '../models/gestion_reparo';
import { GestionReparoService } from '../api/gestionReparoService';

export function useUpsertGestionReparo() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GestionReparo>, unknown, GestionReparo>({
    mutationFn: (payload) => GestionReparoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['gestion_reparo', variables.id_expediente],
      });
    },
  });
}