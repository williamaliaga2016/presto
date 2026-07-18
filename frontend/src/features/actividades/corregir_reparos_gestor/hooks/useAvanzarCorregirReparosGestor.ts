import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparosGestorService } from '../api/corregirReparosGestorService';

export function useAvanzarCorregirReparosGestor() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparosGestorService.avanzar(id_expediente),
  });
}
