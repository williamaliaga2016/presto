import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesValidacionRectificatoriaLegal } from '../models/catalogo';

export function useControlesValidacionRectificatoriaLegal(enabled = true) {
  return useQuery<ApiResponse<ControlesValidacionRectificatoriaLegal>>({
    queryKey: ['validacion_rectificatoria_legal'],
    queryFn: () => catalogoService.getControlesValidacionRectificatoriaLegal(),
    enabled,
    staleTime: 1000 * 60 * 10,
    retry: 1,
  });
}
