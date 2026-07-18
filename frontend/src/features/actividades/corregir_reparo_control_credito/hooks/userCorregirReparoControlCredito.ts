import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirReparoControlCredito } from '../models/corregirReparoControlCredito';
import { corregirReparoControlCreditoService } from '../api/corregirReparoControlCreditoService';

export function useCorregirReparoControlCredito(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoControlCredito | null>>({
    queryKey: ['corregir_reparo_control_credito', id_expediente],
    queryFn: () => corregirReparoControlCreditoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
