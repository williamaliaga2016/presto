import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { reingresarEscrituraCbrService } from '../api/reingresarEscrituraCbrService';

export function useAvanzarReingresarEscrituraCbr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => reingresarEscrituraCbrService.avanzar(id_expediente),
  });
}
