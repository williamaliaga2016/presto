import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidacionRectificatoriaLegalPostventa } from '../models/validacion_rectificatoria_legal_postventa';
import { validacionRectificatoriaLegalPostventaService } from '../api/validacionRectificatoriaLegalPostventaService';

export function useUpsertValidacionRectificatoriaLegalPostventa() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ValidacionRectificatoriaLegalPostventa>, unknown, ValidacionRectificatoriaLegalPostventa>({
    mutationFn: (payload) => validacionRectificatoriaLegalPostventaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['validacion_rectificatoria_legal_postventa', variables.id_expediente],
      });
    },
  });
}
