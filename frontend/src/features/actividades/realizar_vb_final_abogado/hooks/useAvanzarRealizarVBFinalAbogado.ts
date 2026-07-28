import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarVBFinalAbogadoService } from '../api/realizarVBFinalAbogadoService';

export function useAvanzarRealizarVBFinalAbogado() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => realizarVBFinalAbogadoService.avanzar(id_expediente),
  });
}
