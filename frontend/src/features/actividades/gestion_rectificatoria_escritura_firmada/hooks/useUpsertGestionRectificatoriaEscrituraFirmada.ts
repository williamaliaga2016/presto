import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaEscrituraFirmada } from '../models/gestion_rectificatoria_escritura_firmada';
import { GestionRectificatoriaEscrituraFirmadaService } from '../api/gestionRectificatoriaEscrituraFirmadaService';

export function useUpsertGestionRectificatoriaEscrituraFirmada() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GestionRectificatoriaEscrituraFirmada>, unknown, GestionRectificatoriaEscrituraFirmada>({
    mutationFn: (payload) => GestionRectificatoriaEscrituraFirmadaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['gestion_reparo', variables.id_expediente],
      });
    },
  });
}