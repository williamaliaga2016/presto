import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoria } from '../models/gestion_rectificatoria';
import { GestionRectificatoriaService } from '../api/gestionRectificatoriaService';

export function useGestionRectificatoria(idExpediente: number) {
  const query = useQuery<ApiResponse<GestionRectificatoria | null>>({
    queryKey: ['gestion_rectificatoria', idExpediente],
    queryFn: () => GestionRectificatoriaService.getByExpediente(idExpediente),
    enabled: !!idExpediente && idExpediente > 0,
  });

  return {
    data: query.data,
    isLoading: query.isLoading,
    refetch: query.refetch,
  };
}