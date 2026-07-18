import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';

export function useAvanzarRevisarDatosOperacion() {
  return useMutation<ApiResponse<unknown>, unknown, number>({
    mutationFn: (id_expediente) => revisarDatosOperacionService.avanzar(id_expediente),
  });
}
