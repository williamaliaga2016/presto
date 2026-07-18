import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';
import type { ExpedienteDigital } from '../models/ExpedienteDigital';

export function useArchivosExpedienteDigitalByActividad(
  id_expediente: number,
  actividad_id?: string,
) {
  return useQuery<ApiResponse<ExpedienteDigital[]>>({
    queryKey: ['expediente_digital', 'archivos_actividad', id_expediente, actividad_id],
    queryFn: () =>
      expedienteDigitalService.getFilesByActividad(id_expediente, actividad_id!),
    enabled: id_expediente > 0 && !!actividad_id,
  });
}
