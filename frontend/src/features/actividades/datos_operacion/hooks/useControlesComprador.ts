import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';
import type { ControlesComprador } from '../models/catalogo';

export function useControlesComprador(enabled = true) {
  return useQuery<ApiResponse<ControlesComprador>>({
    queryKey: ['datos_operacion_controles_comprador'],
    queryFn: () => datosOperacionService.getControlesComprador(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
