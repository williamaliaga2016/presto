import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { generarPreFiniquitoService } from '../api/generarPreFiniquitoService';

export function useAvanzarGenerarPreFiniquito() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => generarPreFiniquitoService.avanzar(id_expediente),
  });
}