import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';
import type { AvanzarValidarIntegracionResponse } from '../models/validar_integracion_documentos.response';
import { validarIntegracionQueryKeys } from './useValidarIntegracionDocumentos';

export function useAvanzarValidarIntegracionDocumentos() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<AvanzarValidarIntegracionResponse>,
    Error,
    number
  >({
    mutationFn: (id_expediente) =>
      validarIntegracionDocumentosService.avanzar(id_expediente),
    onSuccess: (_, id_expediente) => {
      void queryClient.invalidateQueries({
        queryKey: validarIntegracionQueryKeys.actividad(id_expediente),
      });
    },
  });
}
