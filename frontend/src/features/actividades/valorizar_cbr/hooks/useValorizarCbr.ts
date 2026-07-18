import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValorizarCbr } from '../models/valorizar_cbr';
import { valorizarCbrService } from '../api/valorizarCbrService';

export function useValorizarCbr(id_expediente: number) {
  return useQuery<ApiResponse<ValorizarCbr | null>>({
    queryKey: ['valorizar_cbr', id_expediente],
    queryFn: () => valorizarCbrService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
