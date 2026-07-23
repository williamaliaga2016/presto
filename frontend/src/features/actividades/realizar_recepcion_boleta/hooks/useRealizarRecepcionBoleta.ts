import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GetByExpedienteResponse } from '../models/realizar_recepcion_boleta';
import { realizarRecepcionBoletaService } from '../api/realizarRecepcionBoletaService';

export function useRealizarRecepcionBoleta(id_expediente: number) {
  return useQuery<ApiResponse<GetByExpedienteResponse | null>>({
    queryKey: ['realizar_recepcion_boleta', id_expediente],
    queryFn: () => realizarRecepcionBoletaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
