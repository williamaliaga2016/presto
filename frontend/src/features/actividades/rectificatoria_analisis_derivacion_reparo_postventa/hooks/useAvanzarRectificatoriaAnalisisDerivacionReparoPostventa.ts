import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { rectificatoriaAnalisisDerivacionReparoPostventaService } from '../api/rectificatoriaAnalisisDerivacionReparoPostventaService';

export function useAvanzarRectificatoriaAnalisisDerivacionReparoPostventa() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaAnalisisDerivacionReparoPostventaService.avanzar(id_expediente),
  });
}
