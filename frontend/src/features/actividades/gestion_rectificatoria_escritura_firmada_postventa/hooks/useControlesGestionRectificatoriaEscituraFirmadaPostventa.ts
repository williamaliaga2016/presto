import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import {GestionRectificatoriaEscrituraFirmadaPostventaService } from '../api/gestionRectificatoriaEscrituraFirmadaPostventaService';
import type { ControlesGestionRectificatoriaEscrituraFirmadaPostventa } from '../models/catalogo';

export function useControlesRectificatoriaEscrituraFirmadaPostventa(enabled = true) {
  return useQuery<ApiResponse<ControlesGestionRectificatoriaEscrituraFirmadaPostventa>>({
    queryKey: ['catalogo'],
    queryFn: () => GestionRectificatoriaEscrituraFirmadaPostventaService.getControles(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
