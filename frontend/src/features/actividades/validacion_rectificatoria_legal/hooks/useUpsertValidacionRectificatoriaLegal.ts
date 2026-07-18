import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidacionRectificatoriaLegal } from '../models/validacion_rectificatoria_legal';
import { validacionRectificatoriaLegalService } from '../api/validacionRectificatoriaLegalService';

export function useUpsertValidacionRectificatoriaLegal() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ValidacionRectificatoriaLegal>, unknown, ValidacionRectificatoriaLegal>({
    mutationFn: (payload) => validacionRectificatoriaLegalService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['validacion_rectificatoria_legal', variables.id_expediente],
      });
    },
  });
}
