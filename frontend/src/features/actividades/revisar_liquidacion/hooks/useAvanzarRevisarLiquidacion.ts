import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarLiquidacionService } from '../api/revisarLiquidacionService';

export function useAvanzarRevisarLiquidacion() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => revisarLiquidacionService.avanzar(id_expediente),
  });
}
