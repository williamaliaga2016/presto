import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaSolucionReparo } from '../models/gestion_rectificatoria_solucion_reparo';
import { GestionRectificatoriaSolucionReparoService } from '../api/gestionRectificatoriaSolucionReparoService';

export function useUpsertGestionRectificatoriaSolucionReparo() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GestionRectificatoriaSolucionReparo>, unknown, GestionRectificatoriaSolucionReparo>({
    mutationFn: (payload) => GestionRectificatoriaSolucionReparoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['gestion_rectificatoria_solucion_reparo', variables.id_expediente],
      });
    },
  });
}