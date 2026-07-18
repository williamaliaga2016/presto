import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaEscrituraFirmadaPostventa } from '../models/gestion_rectificatoria_escritura_firmada_postventa';
import { GestionRectificatoriaEscrituraFirmadaPostventaService } from '../api/gestionRectificatoriaEscrituraFirmadaPostventaService';

export function useGestionRectificatoriaEscrituraFirmadaPostventa(idExpediente: number) {
  const query = useQuery<ApiResponse<GestionRectificatoriaEscrituraFirmadaPostventa | null>>({
    queryKey: ['gestion_rectificatoria_escritura_firmada_postventa', idExpediente],
    queryFn: () => GestionRectificatoriaEscrituraFirmadaPostventaService.getByExpediente(idExpediente),
    enabled: !!idExpediente && idExpediente > 0,
  });

  return {
    data: query.data,
    isLoading: query.isLoading,
    refetch: query.refetch,
  };
}