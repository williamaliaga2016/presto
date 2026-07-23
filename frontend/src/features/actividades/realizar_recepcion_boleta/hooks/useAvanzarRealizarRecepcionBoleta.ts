import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarRecepcionBoletaService } from '../api/realizarRecepcionBoletaService';

export function useAvanzarRealizarRecepcionBoleta() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) =>
      realizarRecepcionBoletaService.avanzar(id_expediente),
  });
}
