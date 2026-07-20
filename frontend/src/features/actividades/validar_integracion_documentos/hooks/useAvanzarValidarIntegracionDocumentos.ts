import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';

export function useAvanzarValidarIntegracionDocumentos() {
  return useMutation<ApiResponse<any>, unknown, number>({
    mutationFn: (id_expediente) => validarIntegracionDocumentosService.avanzar(id_expediente),
  });
}