import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';
import type { ControlesBancoAcreedor } from '../models/catalogo';

export function useControlesBancoAcreedor(enabled = true) {
  return useQuery<ApiResponse<ControlesBancoAcreedor>>({
    queryKey: ['datos_operacion_controles_banco_acreedor'],
    queryFn: () => datosOperacionService.getControlesBancoAcreedor(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
