import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validacionRectificatoriaLegalService } from '../api/validacionRectificatoriaLegalService';

export function useAvanzarValidacionRectificatoriaLegal() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => validacionRectificatoriaLegalService.avanzar(id_expediente),
  });
}
