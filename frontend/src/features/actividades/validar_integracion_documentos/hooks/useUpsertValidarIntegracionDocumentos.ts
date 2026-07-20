import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GuardarValidarIntegracionDocumentos, ValidarIntegracionDocumentos } from '../models/validar_integracion_documentos';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';

export function useUpsertValidarIntegracionDocumentos() {
  return useMutation<ApiResponse<ValidarIntegracionDocumentos>, unknown, GuardarValidarIntegracionDocumentos>({
    mutationFn: (payload) => validarIntegracionDocumentosService.save(payload),
  });
}