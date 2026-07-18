import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { ControlesComprador } from '../models/catalogo';

export function useControlesComprador(enabled = true) {
  return useQuery<ApiResponse<ControlesComprador>>({
    queryKey: ['revisar_datos_operacion_controles_comprador'],
    queryFn: () => revisarDatosOperacionService.getControlesComprador(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
