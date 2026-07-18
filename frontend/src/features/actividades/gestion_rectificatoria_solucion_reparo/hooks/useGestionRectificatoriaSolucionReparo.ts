import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionRectificatoriaSolucionReparo } from '../models/gestion_rectificatoria_solucion_reparo';
import { GestionRectificatoriaSolucionReparoService } from '../api/gestionRectificatoriaSolucionReparoService';

export function useGestionRectificatoriaSolucionReparo(idExpediente: number) {
  const query = useQuery<ApiResponse<GestionRectificatoriaSolucionReparo | null>>({
    queryKey: ['gestion_rectificatoria_solucion_reparo', idExpediente],
    queryFn: () => GestionRectificatoriaSolucionReparoService.getByExpediente(idExpediente),
    enabled: !!idExpediente && idExpediente > 0,
  });

  return {
    data: query.data,
    isLoading: query.isLoading,
    refetch: query.refetch,
  };
}