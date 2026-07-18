import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { entregarCarpetaService } from '../api/entregarCarpetaService';

export function useAvanzarEntregarCarpeta() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => entregarCarpetaService.avanzar(id_expediente),
  });
}
