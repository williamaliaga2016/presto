import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesValidacionRectificatoriaLegalPostventa } from '../models/catalogo';

export function useControlesValidacionRectificatoriaLegalPostventa(enabled = true) {
  return useQuery<ApiResponse<ControlesValidacionRectificatoriaLegalPostventa>>({
    queryKey: ['validacion_rectificatoria_legal_postventa'],
    queryFn: () => catalogoService.getControlesValidacionRectificatoriaLegalPostventa(),
    enabled,
    staleTime: 1000 * 60 * 10,
    retry: 1,
  });
}
