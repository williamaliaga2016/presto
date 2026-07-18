import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarDesembolso } from '../models/revisar_desembolso';
import { revisarDesembolsoService } from '../api/revisarDesembolsoService';

export function useRevisarDesembolso(id_expediente: number) {
  return useQuery<ApiResponse<RevisarDesembolso | null>>({
    queryKey: ['revisar_desembolso', id_expediente],
    queryFn: () => revisarDesembolsoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}