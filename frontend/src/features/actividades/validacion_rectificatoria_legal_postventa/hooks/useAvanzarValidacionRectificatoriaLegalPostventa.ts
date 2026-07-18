import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validacionRectificatoriaLegalPostventaService } from '../api/validacionRectificatoriaLegalPostventaService';

export function useAvanzarValidacionRectificatoriaLegalPostventa() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => validacionRectificatoriaLegalPostventaService.avanzar(id_expediente),
  });
}
