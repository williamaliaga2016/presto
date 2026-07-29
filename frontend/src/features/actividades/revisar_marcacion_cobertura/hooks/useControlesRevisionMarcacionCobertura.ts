import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesRevisionMarcacionCobertura } from '../models/controles';
import { revisarMarcacionCoberturaService } from '../api/revisarMarcacionCoberturaService';

export function useControlesRevisionMarcacionCobertura() {
  return useQuery<ApiResponse<ControlesRevisionMarcacionCobertura>>({
    queryKey: ['revision_marcacion_cobertura_controles'],
    queryFn: () => revisarMarcacionCoberturaService.getControles(),
    staleTime: 1000 * 60 * 30, // 30 min — los catálogos no cambian durante la sesión
  });
}
