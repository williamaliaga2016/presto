import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export function useUpsertValidarInformacion() {
  const queryClient = useQueryClient();
  return useMutation<
    ApiResponse<ValidarInformacionBBVA>,
    Error,
    ValidarInformacionBBVA
  >({
    mutationFn: (payload) => validarInformacionService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['validar-informacion', variables.id_expediente],
      });
      queryClient.invalidateQueries({
        queryKey: ['validar-informacion-con-encabezado', variables.id_expediente],
      });
    },
  });
}
