import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { ControlesCredito } from '../models/catalogo';

export function useControlesCredito(enabled = true) {
  return useQuery<ApiResponse<ControlesCredito>>({
    queryKey: ['revisar_datos_operacion_controles_credito'],
    queryFn: () => revisarDatosOperacionService.getControlesCredito(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
