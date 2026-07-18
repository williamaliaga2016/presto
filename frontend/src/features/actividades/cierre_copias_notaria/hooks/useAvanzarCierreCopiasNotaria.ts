import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { cierreCopiasNotariaService } from '../api/cierreCopiasNotariaService';

export function useAvanzarCierreCopiasNotaria() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => cierreCopiasNotariaService.avanzar(id_expediente),
  });
}
