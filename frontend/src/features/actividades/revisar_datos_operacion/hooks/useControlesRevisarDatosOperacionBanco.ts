import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { ControlesRevisarDatosOperacionBanco } from '../models/catalogo';

export function useControlesRevisarDatosOperacionBanco(enabled = true) {
  return useQuery<ApiResponse<ControlesRevisarDatosOperacionBanco>>({
    queryKey: ['revisar_datos_operacion_controles_banco'],
    queryFn: () => revisarDatosOperacionService.getControles(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}