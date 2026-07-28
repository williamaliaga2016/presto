import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ExcepcionDesembolsoResponse } from '../models/excepcion_desembolso';
import { excepcionDesembolsoService } from '../api/excepcionDesembolsoService';

export function useExcepcionDesembolso(id_expediente: number) {
  return useQuery<ApiResponse<ExcepcionDesembolsoResponse | null>>({
    queryKey: ['excepcion_desembolso', id_expediente],
    queryFn: () => excepcionDesembolsoService.getByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
