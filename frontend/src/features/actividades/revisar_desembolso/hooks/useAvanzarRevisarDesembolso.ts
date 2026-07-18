import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDesembolsoService } from '../api/revisarDesembolsoService';

export function useAvanzarRevisarDesembolso() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => revisarDesembolsoService.avanzar(id_expediente),
  });
}