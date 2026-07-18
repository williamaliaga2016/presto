import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarMemoEscritura } from '../models/generar_memo_escritura';
import { memoEscrituraService } from '../api/memoEscrituraService';

export function useGenerarMemoEscritura(id_expediente: number) {
  return useQuery<ApiResponse<GenerarMemoEscritura | null>>({
    queryKey: ['generar_memo_escritura', id_expediente],
    queryFn: () => memoEscrituraService.getByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
