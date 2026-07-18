import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { EncabezadoDTO } from '../models/encabezado';
import { encabezadoService } from '../api/encabezadoService';

export function useEncabezado(idExpediente: number, activityID?: string) {
  return useQuery<ApiResponse<EncabezadoDTO | null>>({
    queryKey: ['encabezado_actividad', idExpediente, activityID ?? null],
    queryFn: () =>
      encabezadoService.getInfoEncabezado(idExpediente, activityID),
    enabled: idExpediente > 0,
  });
}
