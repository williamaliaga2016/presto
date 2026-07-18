import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { verificarCorreccionEscrituraService } from '../api/verificarCorreccionEscrituraService';

export function useAvanzarVerificarCorreccionEscritura() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => verificarCorreccionEscrituraService.avanzar(id_expediente),
  });
}