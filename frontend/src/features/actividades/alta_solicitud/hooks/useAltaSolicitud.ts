import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AltaSolicitud } from '../models/alta_solicitud';
import { altaSolicitudService } from '../api/altaSolicitudService';

export function useAltaSolicitud(id_expediente: number) {
  return useQuery<ApiResponse<AltaSolicitud | null>>({
    queryKey: ['alta_solicitud', id_expediente],
    queryFn: () => altaSolicitudService.getByIdExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
