import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesGenerarBorradorEscritura } from '../models/catalogo';

export function useControlesGenerarBorradorEscritura(enabled = true) {
  return useQuery<ApiResponse<ControlesGenerarBorradorEscritura>>({
    queryKey: ['carga_operacion_banco_controles_datos_operacion'],
    queryFn: () => catalogoService.getControlesGenerarBorradorEscritura(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
