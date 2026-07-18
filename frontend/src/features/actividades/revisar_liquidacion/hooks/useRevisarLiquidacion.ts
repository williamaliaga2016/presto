import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarLiquidacion } from '../models/revisar_liquidacion';
import { revisarLiquidacionService } from '../api/revisarLiquidacionService';

export function useRevisarLiquidacion(id_expediente: number) {
  return useQuery<ApiResponse<RevisarLiquidacion | null>>({
    queryKey: ['revisar_liquidacion', id_expediente],
    queryFn: () => revisarLiquidacionService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
