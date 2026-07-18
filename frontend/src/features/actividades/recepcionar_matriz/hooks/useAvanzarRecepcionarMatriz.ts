import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { recepcionarMatrizService } from '../api/recepcionarMatrizService';

export function useAvanzarRecepcionarMatriz() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => recepcionarMatrizService.avanzar(id_expediente),
  });
}
