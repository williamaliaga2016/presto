import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VisarOperacion } from '../models/visar_operacion';
import { visarOperacionService } from '../api/visarOperacionService';

export function useVisarOperacion(id_expediente: number) {
  return useQuery<ApiResponse<VisarOperacion | null>>({
    queryKey: ['visar_operacion', id_expediente],
    queryFn: () => visarOperacionService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
