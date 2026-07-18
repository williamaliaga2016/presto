import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarCopiasEscriturasService } from '../api/revisarCopiasEscriturasService';

export function useAvanzarRevisarCopiasEscrituras() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => revisarCopiasEscriturasService.avanzar(id_expediente),
  });
}
