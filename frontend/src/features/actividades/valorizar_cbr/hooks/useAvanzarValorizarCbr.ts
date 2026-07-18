import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { valorizarCbrService } from '../api/valorizarCbrService';

export function useAvanzarValorizarCbr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => valorizarCbrService.avanzar(id_expediente),
  });
}
