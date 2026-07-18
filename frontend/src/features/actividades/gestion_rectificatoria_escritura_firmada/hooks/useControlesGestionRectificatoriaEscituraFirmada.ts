import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import {GestionRectificatoriaEscrituraFirmadaService } from '../api/gestionRectificatoriaEscrituraFirmadaService';
import type { ControlesGestionRectificatoriaEscrituraFirmada } from '../models/catalogo';

export function useControlesRectificatoriaEscrituraFirmada(enabled = true) {
  return useQuery<ApiResponse<ControlesGestionRectificatoriaEscrituraFirmada>>({
    queryKey: ['catalogo'],
    queryFn: () => GestionRectificatoriaEscrituraFirmadaService.getControles(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
