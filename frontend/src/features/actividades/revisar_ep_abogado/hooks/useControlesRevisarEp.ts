import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesRevisarEp } from '../models/revisar_ep_abogado';
import { revisarEpAbogadoService } from '../api/revisarEpAbogadoService';

export function useControlesRevisarEp(id_expediente: number) {
  return useQuery<ApiResponse<ControlesRevisarEp>>({
    queryKey: ['revisar_ep_abogado_controles', id_expediente],
    queryFn: () => revisarEpAbogadoService.getControles(id_expediente),
    enabled: !!id_expediente,
    staleTime: 1000 * 60 * 30, // 30 min — los catálogos no cambian durante la sesión
  });
}
