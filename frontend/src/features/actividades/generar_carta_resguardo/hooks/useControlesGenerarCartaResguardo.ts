import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { generarCartaResguardoService } from '../api/generarCartaResguardoService';
import type { ControlesGenerarCartaResguardo } from '../models/catalogo';

export function useControlesGenerarCartaResguardo(enabled = true) {
  return useQuery<ApiResponse<ControlesGenerarCartaResguardo>>({
    queryKey: ['carga_operacion_banco_controles_datos_operacion'],
    queryFn: () => generarCartaResguardoService.getControlesGenerarCartaResguardo(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
