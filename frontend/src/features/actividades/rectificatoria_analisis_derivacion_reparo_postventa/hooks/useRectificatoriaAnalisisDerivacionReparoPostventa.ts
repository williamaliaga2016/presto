import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RectificatoriaAnalisisDerivacionReparoPostventa } from '../models/rectificatoria_analisis_derivacion_reparo_postventa';
import { rectificatoriaAnalisisDerivacionReparoPostventaService } from '../api/rectificatoriaAnalisisDerivacionReparoPostventaService';

export function useRectificatoriaAnalisisDerivacionReparoPostventa(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaAnalisisDerivacionReparoPostventa | null>>({
    queryKey: ['rectificatoria_analisis_derivacion_reparo_postventa', id_expediente],
    queryFn: () => rectificatoriaAnalisisDerivacionReparoPostventaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
