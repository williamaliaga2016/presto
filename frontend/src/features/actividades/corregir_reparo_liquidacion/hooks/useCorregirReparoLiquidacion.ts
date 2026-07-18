import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoLiquidacion } from '../models/corregir_reparo_liquidacion';
import { corregirReparoLiquidacionService } from '../api/corregirReparoLiquidacionService';

export function useCorregirReparoLiquidacion(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoLiquidacion | null>>({
    queryKey: ['corregir_reparo_liquidacion', id_expediente],
    queryFn: () => corregirReparoLiquidacionService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
