import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlEscritura } from '../models/control_escritura';
import { controlEscrituraService } from '../api/controlEscrituraService';

export function useControlEscritura(id_expediente: number) {
  return useQuery<ApiResponse<ControlEscritura | null>>({
    queryKey: ['control_escritura', id_expediente],
    queryFn: () => controlEscrituraService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
