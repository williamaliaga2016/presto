import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';
import type { ExpedienteDigital } from '../models/ExpedienteDigital';

/**
 * Consulta los archivos del expediente digital con filtro opcional por actividad.
 *
 * @param id_expediente Identificador del expediente.
 * @param activity_id Actividad workflow usada para limitar el listado.
 * @returns Query de React Query con los archivos del expediente digital.
 */
export function useArchivosExpedienteDigital(
  id_expediente: number,
  activity_id?: string,
) {
  return useQuery<ApiResponse<ExpedienteDigital[]>>({
    queryKey: ['expediente_digital', 'archivos', id_expediente, activity_id ?? 'all'],
    queryFn: () =>
      expedienteDigitalService.getFilesExpedienteDigital(
        id_expediente,
        activity_id,
      ),
    enabled: id_expediente > 0,
  });
}
