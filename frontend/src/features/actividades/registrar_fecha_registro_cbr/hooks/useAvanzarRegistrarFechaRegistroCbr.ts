import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { registrarFechaRegistroCbrService } from '../api/registrarFechaRegistroCbrService';

export function useAvanzarRegistrarFechaRegistroCbr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => registrarFechaRegistroCbrService.avanzar(id_expediente),
  });
}
