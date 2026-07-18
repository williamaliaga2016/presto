import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';

export function useAvanzarDatosOperacion() {
  return useMutation<ApiResponse<unknown>, unknown, number>({
    mutationFn: (id_expediente) => datosOperacionService.avanzar(id_expediente),
  });
}
