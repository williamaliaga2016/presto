import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { generarFiniquitoService } from '../api/generarFiniquitoService';

export function useAvanzarGenerarFiniquito() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => generarFiniquitoService.avanzar(id_expediente),
  });
}
