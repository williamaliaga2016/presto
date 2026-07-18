import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidacionRectificatoriaLegalPostventa } from '../models/validacion_rectificatoria_legal_postventa';
import { validacionRectificatoriaLegalPostventaService } from '../api/validacionRectificatoriaLegalPostventaService';

export function useValidacionRectificatoriaLegalPostventa(id_expediente: number) {
  return useQuery<ApiResponse<ValidacionRectificatoriaLegalPostventa | null>>({
    queryKey: ['validacion_rectificatoria_legal_postventa', id_expediente],
    queryFn: () => validacionRectificatoriaLegalPostventaService.getFullByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
