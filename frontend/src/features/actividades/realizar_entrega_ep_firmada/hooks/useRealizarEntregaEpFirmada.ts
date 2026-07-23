import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GetByExpedienteResponse } from '../models/realizar_entrega_ep_firmada';
import { realizarEntregaEpFirmadaService } from '../api/realizarEntregaEpFirmadaService';

export function useRealizarEntregaEpFirmada(id_expediente: number) {
  return useQuery<ApiResponse<GetByExpedienteResponse | null>>({
    queryKey: ['realizar_entrega_ep_firmada', id_expediente],
    queryFn: () => realizarEntregaEpFirmadaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
