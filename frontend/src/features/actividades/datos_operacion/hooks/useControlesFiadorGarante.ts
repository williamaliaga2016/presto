import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';
import type { ControlesFiadorGarante } from '../models/catalogo';

export function useControlesFiadorGarante(enabled = true) {
  return useQuery<ApiResponse<ControlesFiadorGarante>>({
    queryKey: ['datos_operacion_controles_fiador_garante'],
    queryFn: () => datosOperacionService.getControlesFiadorGarante(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
