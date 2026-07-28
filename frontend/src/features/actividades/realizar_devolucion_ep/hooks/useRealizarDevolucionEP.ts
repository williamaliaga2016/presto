import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GetByExpedienteResponse } from '../models/realizar_devolucion_ep';
import { realizarDevolucionEPService } from '../api/realizarDevolucionEPService';

export function useRealizarDevolucionEP(id_expediente: number) {
  return useQuery<ApiResponse<GetByExpedienteResponse | null>>({
    queryKey: ['realizar_devolucion_ep', id_expediente],
    queryFn: () => realizarDevolucionEPService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
    staleTime: 0,
    refetchOnMount: 'always',
  });
}
