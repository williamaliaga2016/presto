import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarIngresoDatosCredito } from '../models/revisar_ingreso_datos_credito';
import { revisarIngresoDatosCreditoService } from '../api/revisarIngresoDatosCreditoService';

export function useRevisarIngresoDatosCredito(id_expediente: number) {
  return useQuery<ApiResponse<RevisarIngresoDatosCredito | null>>({
    queryKey: ['revisar_ingreso_datos_credito', id_expediente],
    queryFn: () => revisarIngresoDatosCreditoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
  
}
