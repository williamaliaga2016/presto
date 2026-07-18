import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CierreCopiasNotaria } from '../models/cierre_copias_notaria';
import { cierreCopiasNotariaService } from '../api/cierreCopiasNotariaService';

export function useCierreCopiasNotaria(id_expediente: number) {
  return useQuery<ApiResponse<CierreCopiasNotaria | null>>({
    queryKey: ['cierre_copias_notaria', id_expediente],
    queryFn: () => cierreCopiasNotariaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
