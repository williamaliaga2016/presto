import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { calculoGeneracionDocumentoService } from '../api/calculoGeneracionDocumentoService';

export function useAvanzarCalculoGeneracionDocumento() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => calculoGeneracionDocumentoService.avanzar(id_expediente),
  });
}
