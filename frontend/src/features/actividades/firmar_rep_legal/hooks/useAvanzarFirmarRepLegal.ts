import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useAvanzarFirmarRepLegal() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) =>
      firmarRepLegalService.avanzar(id_expediente),
  });
}
