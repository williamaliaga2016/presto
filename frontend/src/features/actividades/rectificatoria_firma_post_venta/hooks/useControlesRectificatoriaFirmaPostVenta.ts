import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesRectificatoriaFirmaPostVenta } from '../models/catalogo';

export function useControlesRectificatoriaFirmaPostVenta(enabled = true) {
  return useQuery<ApiResponse<ControlesRectificatoriaFirmaPostVenta>>({
    queryKey: ['recificacion_firma_post_venta_controles'],
    queryFn: () => catalogoService.getControlesRectificatoriaFirmaPostVenta(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
