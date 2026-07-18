import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesRectificatoriaFirma } from '../models/catalogo';

export function useControlesRectificatoriaFirma(enabled = true) {
  return useQuery<ApiResponse<ControlesRectificatoriaFirma>>({
    queryKey: ['recificacion_firma_controles'],
    queryFn: () => catalogoService.getControlesRectificatoriaFirma(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
