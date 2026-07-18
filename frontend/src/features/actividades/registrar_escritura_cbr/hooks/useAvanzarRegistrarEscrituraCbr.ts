import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { registrarEscrituraCbrService } from '../api/registrarEscrituraCbrService';

export function useAvanzarRegistrarEscrituraCbr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => registrarEscrituraCbrService.avanzar(id_expediente),
  });
}
