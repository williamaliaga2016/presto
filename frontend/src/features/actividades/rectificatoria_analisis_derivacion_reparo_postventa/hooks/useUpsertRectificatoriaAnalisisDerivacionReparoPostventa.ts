import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RectificatoriaAnalisisDerivacionReparoPostventa } from '../models/rectificatoria_analisis_derivacion_reparo_postventa';
import { rectificatoriaAnalisisDerivacionReparoPostventaService } from '../api/rectificatoriaAnalisisDerivacionReparoPostventaService';

export function useUpsertRectificatoriaAnalisisDerivacionReparoPostventa() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RectificatoriaAnalisisDerivacionReparoPostventa>, unknown, RectificatoriaAnalisisDerivacionReparoPostventa>({
    mutationFn: (payload) => rectificatoriaAnalisisDerivacionReparoPostventaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['rectificatoria_analisis_derivacion_reparo_postventa', variables.id_expediente],
      });
    },
  });
}
