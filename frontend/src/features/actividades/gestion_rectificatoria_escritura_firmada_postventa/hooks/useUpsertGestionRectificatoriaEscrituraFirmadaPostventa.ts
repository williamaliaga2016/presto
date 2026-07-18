import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaEscrituraFirmadaPostventa } from '../models/gestion_rectificatoria_escritura_firmada_postventa';
import { GestionRectificatoriaEscrituraFirmadaPostventaService } from '../api/gestionRectificatoriaEscrituraFirmadaPostventaService';

export function useUpsertGestionRectificatoriaEscrituraFirmadaPostventa() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<GestionRectificatoriaEscrituraFirmadaPostventa>, unknown, GestionRectificatoriaEscrituraFirmadaPostventa>({
    mutationFn: (payload) => GestionRectificatoriaEscrituraFirmadaPostventaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['gestion_rectificatoria_escritura_firmada_postventa', variables.id_expediente],
      });
    },
  });
}