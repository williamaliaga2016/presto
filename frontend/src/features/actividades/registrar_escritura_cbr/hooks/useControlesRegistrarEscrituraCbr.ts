import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { registrarEscrituraCbrService } from '../api/registrarEscrituraCbrService';
import type { ControlesRegistrarEscrituraCbr } from '../models/catalogo';

export function useControlesRegistrarEscrituraCbr(enabled = true) {
  return useQuery<ApiResponse<ControlesRegistrarEscrituraCbr>>({
    queryKey: ['registrar_escritura_cbr'],
    queryFn: () => registrarEscrituraCbrService.getControlesRegistrarEscrituraCbr(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
