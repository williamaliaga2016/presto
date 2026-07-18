import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CalculoGeneracionDocumento } from '../models/calculo_generacion_documento';
import { calculoGeneracionDocumentoService } from '../api/calculoGeneracionDocumentoService';

export function useUpsertCalculoGeneracionDocumento() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CalculoGeneracionDocumento>, unknown, CalculoGeneracionDocumento>({
    mutationFn: (payload) => calculoGeneracionDocumentoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['calculo_generacion_documento', variables.id_expediente],
      });
    },
  });
}
