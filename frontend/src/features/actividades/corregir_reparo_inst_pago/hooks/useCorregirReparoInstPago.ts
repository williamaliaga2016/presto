import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoInstPago } from '../models/corregir_reparo_inst_pago';
import { corregirReparoInstPagoService } from '../api/corregirReparoInstPagoService';

export function useCorregirReparoInstPago(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoInstPago | null>>({
    queryKey: ['corregir_reparo_inst_pago', id_expediente],
    queryFn: () => corregirReparoInstPagoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
