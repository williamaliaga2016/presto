import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';
import type { ControlesPropiedad } from '../models/catalogo';

export function useControlesPropiedad(enabled = true) {
  return useQuery<ApiResponse<ControlesPropiedad>>({
    queryKey: ['datos_operacion_controles_propiedad'],
    queryFn: () => datosOperacionService.getControlesPropiedad(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
