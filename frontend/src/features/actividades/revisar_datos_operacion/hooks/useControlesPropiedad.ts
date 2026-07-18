import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { ControlesPropiedad } from '../models/catalogo';

export function useControlesPropiedad(enabled = true) {
  return useQuery<ApiResponse<ControlesPropiedad>>({
    queryKey: ['revisar_datos_operacion_controles_propiedad'],
    queryFn: () => revisarDatosOperacionService.getControlesPropiedad(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
