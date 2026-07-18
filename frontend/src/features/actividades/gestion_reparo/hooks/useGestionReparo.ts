import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionReparo } from '../models/gestion_reparo';
import { GestionReparoService } from '../api/gestionReparoService';

export function useGestionReparo(idExpediente: number) {
  const query = useQuery<ApiResponse<GestionReparo | null>>({
    queryKey: ['gestion_reparo', idExpediente],
    queryFn: () => GestionReparoService.getByExpediente(idExpediente),
    enabled: !!idExpediente && idExpediente > 0,
  });

  return {
    data: query.data,
    isLoading: query.isLoading,
    refetch: query.refetch,
  };
}