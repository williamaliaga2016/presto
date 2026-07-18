import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { GestionRectificatoriaEscrituraFirmadaService } from '../api/gestionRectificatoriaEscrituraFirmadaService';

export function useAvanzarGestionRectificatoriaEscrituraFirmada() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => GestionRectificatoriaEscrituraFirmadaService.avanzar(id_expediente),
  });
}