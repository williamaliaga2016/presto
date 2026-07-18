import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosOperacionService } from '../api/datosOperacionService';
import type { DatosOperacion } from '../models/datos_operacion';

export function useDatosOperacion(id_expediente: number) {
  return useQuery<ApiResponse<DatosOperacion | null>>({
    queryKey: ['datos_operacion_5_4', id_expediente],
    queryFn: () => datosOperacionService.getFullByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
