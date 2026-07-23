import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';
import { revisarEpAbogadoService } from '../api/revisarEpAbogadoService';

export function useRevisarEpAbogado(id_expediente: number) {
  return useQuery<ApiResponse<RevisarEpAbogado | null>>({
    queryKey: ['revisar_ep_abogado', id_expediente],
    queryFn: () => revisarEpAbogadoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
