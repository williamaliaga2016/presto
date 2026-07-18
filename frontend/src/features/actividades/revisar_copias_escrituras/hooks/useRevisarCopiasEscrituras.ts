import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarCopiasEscrituras } from '../models/revisar_copias_escritura';
import { revisarCopiasEscriturasService } from '../api/revisarCopiasEscriturasService';

export function useRevisarCopiasEscrituras(id_expediente: number) {
  return useQuery<ApiResponse<RevisarCopiasEscrituras | null>>({
    queryKey: ['revisar_copias_escritura', id_expediente],
    queryFn: () => revisarCopiasEscriturasService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
