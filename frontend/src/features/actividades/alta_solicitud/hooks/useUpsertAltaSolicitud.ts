import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AltaSolicitud } from '../models/alta_solicitud';
import { altaSolicitudService } from '../api/altaSolicitudService';

export function useUpsertAltaSolicitud() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<AltaSolicitud>, unknown, AltaSolicitud>({
    mutationFn: (payload) => altaSolicitudService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['alta_solicitud', variables.id_expediente],
      });
    },
  });
}
