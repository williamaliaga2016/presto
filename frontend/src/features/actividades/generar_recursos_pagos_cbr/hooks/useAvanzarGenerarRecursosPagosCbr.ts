import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { generarRecursosPagosCbrService } from '../api/generarRecursosPagosCbrService';

export function useAvanzarGenerarRecursosPagosCbr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => generarRecursosPagosCbrService.avanzar(id_expediente),
  });
}
