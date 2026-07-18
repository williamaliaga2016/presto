import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarInscripcionCbrService } from '../api/revisarInscripcionCbrService';

export function useAvanzarRevisarInscripcionCbr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => revisarInscripcionCbrService.avanzar(id_expediente),
  });
}
