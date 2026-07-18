import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { recibirInstruccionPagoService } from '../api/recibirInstruccionPagoService';
import type { ControlesRecibirInstruccionPago } from '../models/catalogo';

export function useControlesRecibirInstruccionPago(enabled = true) {
  return useQuery<ApiResponse<ControlesRecibirInstruccionPago>>({
    queryKey: ['recibir_instruccion_pago'],
    queryFn: () => recibirInstruccionPagoService.getControlesRecibirInstruccionPago(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
