import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaEscrituraFirmada } from '../models/gestion_rectificatoria_escritura_firmada';
import { GestionRectificatoriaEscrituraFirmadaService } from '../api/gestionRectificatoriaEscrituraFirmadaService';

export function useGestionRectificatoriaEscrituraFirmada(idExpediente: number) {
  const query = useQuery<ApiResponse<GestionRectificatoriaEscrituraFirmada | null>>({
    queryKey: ['gestion_rectificatoria_escritura_firmada', idExpediente],
    queryFn: () => GestionRectificatoriaEscrituraFirmadaService.getByExpediente(idExpediente),
    enabled: !!idExpediente && idExpediente > 0,
  });

  return {
    data: query.data,
    isLoading: query.isLoading,
    refetch: query.refetch,
  };
}