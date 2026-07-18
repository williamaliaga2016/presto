import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RecibirInstruccionPago } from '../models/recibir_instruccion_pago';
import { recibirInstruccionPagoService } from '../api/recibirInstruccionPagoService';

export function useRecibirInstruccionPago(id_expediente: number) {
  return useQuery<ApiResponse<RecibirInstruccionPago | null>>({
    queryKey: ['recibir_instruccion_pago', id_expediente],
    queryFn: () => recibirInstruccionPagoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
