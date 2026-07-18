import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';
import { validarInformacionQueryKeys } from './useValidarInformacion';

export function useUpsertValidarInformacion() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<ValidarInformacionBBVA>,
    Error,
    ValidarInformacionBBVA
  >({
    mutationFn: (payload) => validarInformacionService.guardar(payload),
    onSuccess: (_, variables) => {
      void queryClient.invalidateQueries({
        queryKey: validarInformacionQueryKeys.actividad(
          variables.id_expediente,
        ),
      });
    },
  });
}
