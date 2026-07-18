import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ReingresarEscrituraCbr } from '../models/reingresar_escritura_cbr';
import { reingresarEscrituraCbrService } from '../api/reingresarEscrituraCbrService';

export function useReingresarEscrituraCbr(id_expediente: number) {
  return useQuery<ApiResponse<ReingresarEscrituraCbr | null>>({
    queryKey: ['reingresar_escritura_cbr', id_expediente],
    queryFn: () => reingresarEscrituraCbrService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
