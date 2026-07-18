import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { historialExpedienteService } from '../api/historialExpedienteService';
import type { HistorialExpediente } from '../models/historialExpediente';

export function useHistorialExpediente(id_expediente: number) {
  return useQuery<ApiResponse<HistorialExpediente[]>>({
    queryKey: ['historial_expediente', id_expediente],
    queryFn: () => historialExpedienteService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
