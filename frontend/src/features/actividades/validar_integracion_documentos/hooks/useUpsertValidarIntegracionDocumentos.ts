import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type {
  ValidarIntegracionDocumentosData,
} from '../models/validar_integracion_documentos';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';
import { validarIntegracionQueryKeys } from './useValidarIntegracionDocumentos';

export function useUpsertValidarIntegracionDocumentos() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<ValidarIntegracionDocumentosData>,
    Error,
    ValidarIntegracionDocumentosData
  >({
    mutationFn: (payload) => validarIntegracionDocumentosService.save(payload),
    onSuccess: (_, variables) => {
      void queryClient.invalidateQueries({
        queryKey: validarIntegracionQueryKeys.actividad(
          variables.idExpediente,
        ),
      });
    },
  });
}
