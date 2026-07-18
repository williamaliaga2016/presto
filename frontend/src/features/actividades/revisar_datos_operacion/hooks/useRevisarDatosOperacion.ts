import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { RevisarDatosOperacion } from '../models/revisar_datos_operacion';

export function useRevisarDatosOperacion(id_expediente: number) {
  return useQuery<ApiResponse<RevisarDatosOperacion | null>>({
    queryKey: ['revisar_datos_operacion', id_expediente],
    queryFn: () => revisarDatosOperacionService.getFullByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
