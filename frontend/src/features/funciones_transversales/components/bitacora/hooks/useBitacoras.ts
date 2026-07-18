import { useQuery } from '@tanstack/react-query';
import { bitacoraService } from '../api/bitacoraService';
import type { Bitacora } from '../models/bitacora';
import type { ApiResponse } from '@/core/api/models/ApiResponse';

export function useBitacoras(id_expediente: number) {
  return useQuery<ApiResponse<Bitacora[]>>({
    queryKey: ['bitacora', id_expediente],
    queryFn: () => bitacoraService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
