import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirReparoInstPagoService } from '../api/corregirReparoInstPagoService';

export function useAvanzarCorregirReparoInstPago() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoInstPagoService.avanzar(id_expediente),
  });
}
