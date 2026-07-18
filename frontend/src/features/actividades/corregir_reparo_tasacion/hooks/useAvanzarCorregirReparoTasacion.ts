import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparoTasacionService } from '../api/corregirReparoTasacionService';

export function useAvanzarCorregirReparoTasacion() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoTasacionService.avanzar(id_expediente),
  });
}
