import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { MemoEscrituraVersion } from '../models/memo_escritura';
import { memoEscrituraService } from '../api/memoEscrituraService';

export function useVersionesMemoEscritura(id_expediente: number) {
  return useQuery<ApiResponse<MemoEscrituraVersion[]>>({
    queryKey: ['memo_escritura_versiones', id_expediente],
    queryFn: () => memoEscrituraService.listarVersiones(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
