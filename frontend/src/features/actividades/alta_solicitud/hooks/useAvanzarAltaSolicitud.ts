import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { altaSolicitudService } from '../api/altaSolicitudService';

export function useAvanzarAltaSolicitud() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => altaSolicitudService.avanzar(id_expediente),
  });
}
