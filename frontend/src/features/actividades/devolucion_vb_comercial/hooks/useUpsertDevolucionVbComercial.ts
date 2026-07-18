import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  DevolucionVbComercialData,
} from '../models/devolucion_vb_comercial';
import { devolucionVbComercialService } from '../api/devolucionVbComercialService';
import { devolucionVbComercialQueryKeys } from './useDevolucionVbComercial';

export function useUpsertDevolucionVbComercial() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<DevolucionVbComercialData>,
    Error,
    DevolucionVbComercialData
  >({
    mutationFn: (payload) => devolucionVbComercialService.save(payload),
    onSuccess: (_, variables) => {
      void queryClient.invalidateQueries({
        queryKey: devolucionVbComercialQueryKeys.actividad(
          variables.idExpediente,
        ),
      });
    },
  });
}
