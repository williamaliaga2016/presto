import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { asignarEscrituraService } from '../api/asignarEscrituraService';

export function useAvanzarAsignarEscritura() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => asignarEscrituraService.avanzar(id_expediente),
  });
}