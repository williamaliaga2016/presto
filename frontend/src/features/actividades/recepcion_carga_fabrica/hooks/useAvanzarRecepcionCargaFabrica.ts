import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { recepcionCargaFabricaService } from '../api/recepcionCargaFabricaService';

export function useAvanzarRecepcionCargaFabrica() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => recepcionCargaFabricaService.avanzar(id_expediente),
  });
}
