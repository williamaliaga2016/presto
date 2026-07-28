import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarVoboGerenciaCohService } from '../api/realizarVoboGerenciaCohService';

export function useAvanzarVoboGerenciaCoh() {
  return useMutation<
    ApiResponse<{ actividad_destino: string } | null>,
    unknown,
    number
  >({
    mutationFn: (id_expediente) =>
      realizarVoboGerenciaCohService.avanzar(id_expediente),
  });
}
