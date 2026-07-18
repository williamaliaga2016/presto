import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { verificarReparoCbrService } from '../api/verificarReparoCbrService';

export function useAvanzarVerificarReparoCbr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => verificarReparoCbrService.avanzar(id_expediente),
  });
}
