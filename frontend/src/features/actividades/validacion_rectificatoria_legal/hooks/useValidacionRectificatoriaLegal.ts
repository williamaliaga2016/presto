import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidacionRectificatoriaLegal } from '../models/validacion_rectificatoria_legal';
import { validacionRectificatoriaLegalService } from '../api/validacionRectificatoriaLegalService';

export function useValidacionRectificatoriaLegal(id_expediente: number) {
  return useQuery<ApiResponse<ValidacionRectificatoriaLegal | null>>({
    queryKey: ['validacion_rectificatoria_legal', id_expediente],
    queryFn: () => validacionRectificatoriaLegalService.getFullByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
