import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesFirmarEscritura } from '../models/firmar_escritura_cliente';
import { firmarEscrituraClienteService } from '../api/firmarEscrituraClienteService';

export function useControlesFirmarEscritura() {
  return useQuery<ApiResponse<ControlesFirmarEscritura>>({
    queryKey: ['firmar_escritura_cliente_controles'],
    queryFn: () => firmarEscrituraClienteService.getControles(),
    staleTime: 1000 * 60 * 30, // 30 min — los catálogos no cambian durante la sesión
  });
}
