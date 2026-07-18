import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { rectificatoriaAnalisisDerivacionReparoPostventaService } from '../api/rectificatoriaAnalisisDerivacionReparoPostventaService';
import type { ControlesRectificatoriaAnalisisDerivacionReparoPostventa } from '../models/catalogo';

export function useControlesRectificatoriaAnalisisDerivacionReparoPostventa(enabled = true) {
  return useQuery<ApiResponse<ControlesRectificatoriaAnalisisDerivacionReparoPostventa>>({
    queryKey: ['controles_rectificatoria_analisis_derivacion_reparo_postventa'],
    queryFn: () => rectificatoriaAnalisisDerivacionReparoPostventaService.getControlesRectificatoriaAnalisisDerivacionReparoPostventa(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
