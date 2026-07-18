import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparoLiquidacionService } from '../api/corregirReparoLiquidacionService';

export function useAvanzarCorregirReparoLiquidacion() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoLiquidacionService.avanzar(id_expediente),
  });
}
