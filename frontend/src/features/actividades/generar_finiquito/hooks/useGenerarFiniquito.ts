import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarFiniquito } from '../models/generar_finiquito';
import { generarFiniquitoService } from '../api/generarFiniquitoService';

export function useGenerarFiniquito(idExpediente: number) {
  const query = useQuery<ApiResponse<GenerarFiniquito | null>>({
    queryKey: ['generar_finiquito', idExpediente],
    queryFn: () => generarFiniquitoService.getByExpediente(idExpediente),
    enabled: !!idExpediente && idExpediente > 0,
  });

  return {
    data: query.data,
    isLoading: query.isLoading,
    refetch: query.refetch,
  };
}