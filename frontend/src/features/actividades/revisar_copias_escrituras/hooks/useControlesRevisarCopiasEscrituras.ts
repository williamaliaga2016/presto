import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarCopiasEscriturasService } from '../api/revisarCopiasEscriturasService';
import type { ControlesRevisarCopiasEscrituras } from '../models/catalogo';

export function useControlesRevisarCopiasEscrituras(enabled = true) {
  return useQuery<ApiResponse<ControlesRevisarCopiasEscrituras>>({
    queryKey: ['carga_operacion_banco_controles_datos_operacion'],
    queryFn: () => revisarCopiasEscriturasService.getControlesRevisarCopiasEscrituras(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
