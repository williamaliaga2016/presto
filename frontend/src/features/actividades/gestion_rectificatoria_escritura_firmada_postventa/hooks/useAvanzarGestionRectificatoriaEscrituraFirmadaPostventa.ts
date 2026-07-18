import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { GestionRectificatoriaEscrituraFirmadaPostventaService } from '../api/gestionRectificatoriaEscrituraFirmadaPostventaService';

export function useAvanzarGestionRectificatoriaEscrituraFirmadaPostventa() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => GestionRectificatoriaEscrituraFirmadaPostventaService.avanzar(id_expediente),
  });
}