import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparoPrefiniquitoService } from '../api/corregirReparoPrefiniquitoService';

export function useAvanzarCorregirReparoPrefiniquito() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoPrefiniquitoService.avanzar(id_expediente),
  });
}
