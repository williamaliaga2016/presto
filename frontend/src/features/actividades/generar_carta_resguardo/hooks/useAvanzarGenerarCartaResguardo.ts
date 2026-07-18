import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { generarCartaResguardoService } from '../api/generarCartaResguardoService';

export function useAvanzarGenerarCartaResguardo() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => generarCartaResguardoService.avanzar(id_expediente),
  });
}
