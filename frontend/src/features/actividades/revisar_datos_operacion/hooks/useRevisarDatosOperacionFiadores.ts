import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { RevisarDatosOperacionFiadorGarante } from '../models/revisar_datos_operacion';

export function useRevisarDatosOperacionFiadores(id_expediente: number) {
  return useQuery<ApiResponse<RevisarDatosOperacionFiadorGarante[]>>({
    queryKey: ['revisar_datos_operacion_fiadores', id_expediente],
    queryFn: () => revisarDatosOperacionService.getFiadoresByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
