import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisionMarcacionCoberturaResponse } from '../models/revision_marcacion_cobertura';
import { revisarMarcacionCoberturaService } from '../api/revisarMarcacionCoberturaService';

export function useRevisionMarcacionCobertura(id_expediente: number) {
  return useQuery<ApiResponse<RevisionMarcacionCoberturaResponse | null>>({
    queryKey: ['revision_marcacion_cobertura', id_expediente],
    queryFn: () => revisarMarcacionCoberturaService.getByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
