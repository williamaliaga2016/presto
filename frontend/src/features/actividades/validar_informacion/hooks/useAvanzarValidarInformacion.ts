import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validarInformacionService } from '../api/validarInformacionService';
import type { AvanzarValidarInformacionResponse } from
  '../models/avanzar_validar_informacion';
import { validarInformacionQueryKeys } from './useValidarInformacion';

export function useAvanzarValidarInformacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<AvanzarValidarInformacionResponse>, Error, number>({
    mutationFn: (id_expediente) =>
      validarInformacionService.avanzar(id_expediente),
    onSuccess: (_, id_expediente) => {
      void queryClient.invalidateQueries({
        queryKey: validarInformacionQueryKeys.actividad(id_expediente),
      });
    },
  });
}
