import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarGestionComercialService } from '../api/realizarGestionComercialService';

export function useAvanzarRealizarGestionComercial() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => realizarGestionComercialService.avanzar(id_expediente),
  });
}
