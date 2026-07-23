import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarRepLegal } from '../models/firmar_rep_legal';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useFirmarRepLegal(id_expediente: number) {
  return useQuery<ApiResponse<FirmarRepLegal | null>>({
    queryKey: ['firmar_rep_legal', id_expediente],
    queryFn: () => firmarRepLegalService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
