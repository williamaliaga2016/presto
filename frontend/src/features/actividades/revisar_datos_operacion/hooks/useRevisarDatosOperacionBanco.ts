import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { RevisarDatosOperacionBanco } from '../models/revisar_datos_operacion';

export function useRevisarDatosOperacionBanco(id_expediente: number) {
  return useQuery<ApiResponse<RevisarDatosOperacionBanco | null>>({
    queryKey: ['revisar_datos_operacion_banco', id_expediente],
    queryFn: () => revisarDatosOperacionService.getRevisarDatosOperacionBancoByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}