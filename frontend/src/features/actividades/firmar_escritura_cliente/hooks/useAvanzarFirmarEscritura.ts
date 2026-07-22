import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { firmarEscrituraClienteService } from '../api/firmarEscrituraClienteService';

export function useAvanzarFirmarEscritura() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) =>
      firmarEscrituraClienteService.avanzar(id_expediente),
  });
}
