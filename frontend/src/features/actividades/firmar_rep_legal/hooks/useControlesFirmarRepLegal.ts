import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesFirmarRepLegal } from '../models/controles';
import { firmarRepLegalService } from '../api/firmarRepLegalService';

export function useControlesFirmarRepLegal() {
  return useQuery<ApiResponse<ControlesFirmarRepLegal>>({
    queryKey: ['firmar_rep_legal_controles'],
    queryFn: () => firmarRepLegalService.getControles(),
    staleTime: 1000 * 60 * 30, // 30 min — los catálogos no cambian durante la sesión
  });
}
