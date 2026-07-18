import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { visarOperacionService } from '../api/visarOperacionService';

export function useAvanzarVisarOperacion() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => visarOperacionService.avanzar(id_expediente),
  });
}
