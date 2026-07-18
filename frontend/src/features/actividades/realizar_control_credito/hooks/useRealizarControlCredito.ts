import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarControlCredito } from '../models/realizar_control_credito';
import { realizarControlCreditoService } from '../api/realizarControlCreditoService';

export function useRealizarControlCredito(id_expediente: number) {
  return useQuery<ApiResponse<RealizarControlCredito | null>>({
    queryKey: ['realizar_control_credito', id_expediente],
    queryFn: () => realizarControlCreditoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
