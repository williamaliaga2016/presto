import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { generarEstudioTitulosService } from '../api/generarEstudioTitulosService';

export function useAvanzarGenerarEstudioTitulos() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => generarEstudioTitulosService.avanzar(id_expediente),
  });
}