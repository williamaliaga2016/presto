import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarEntregaEpFirmadaService } from '../api/realizarEntregaEpFirmadaService';

export function useAvanzarRealizarEntregaEpFirmada() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) =>
      realizarEntregaEpFirmadaService.avanzar(id_expediente),
  });
}
