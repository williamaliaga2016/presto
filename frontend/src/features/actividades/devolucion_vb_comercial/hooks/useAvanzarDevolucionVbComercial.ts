import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { devolucionVbComercialService } from '../api/devolucionVbComercialService';
import type {
  AvanzarDevolucionVbComercialRequest,
  AvanzarDevolucionVbComercialResponse,
} from '../models/devolucion_vb_comercial.response';
import { devolucionVbComercialQueryKeys } from './useDevolucionVbComercial';

export function useAvanzarDevolucionVbComercial() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<AvanzarDevolucionVbComercialResponse>,
    Error,
    AvanzarDevolucionVbComercialRequest
  >({
    mutationFn: ({ id_expediente, confirmarCierre }) =>
      devolucionVbComercialService.avanzar(
        id_expediente,
        confirmarCierre,
      ),
    onSuccess: (_, variables) => {
      void queryClient.invalidateQueries({
        queryKey: devolucionVbComercialQueryKeys.actividad(
          variables.id_expediente,
        ),
      });
    },
  });
}
