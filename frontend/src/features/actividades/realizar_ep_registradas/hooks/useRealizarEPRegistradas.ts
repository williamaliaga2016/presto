import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GetByExpedienteResponse } from '../models/realizar_ep_registradas';
import { realizarEPRegistradasService } from '../api/realizarEPRegistradasService';

export function useRealizarEPRegistradas(id_expediente: number) {
  return useQuery<ApiResponse<GetByExpedienteResponse | null>>({
    queryKey: ['realizar_ep_registradas', id_expediente],
    queryFn: () => realizarEPRegistradasService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
