import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { recibirInstruccionPagoService } from '../api/recibirInstruccionPagoService';

export function useAvanzarRecibirInstruccionPago() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => recibirInstruccionPagoService.avanzar(id_expediente),
  });
}
