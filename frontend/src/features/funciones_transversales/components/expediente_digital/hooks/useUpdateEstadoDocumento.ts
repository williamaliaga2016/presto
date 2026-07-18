import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';
import type { ExpedienteDigital } from '../models/ExpedienteDigital';

export function useUpdateEstadoDocumento() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ExpedienteDigital>, unknown, ExpedienteDigital>({
    mutationFn: (payload) => expedienteDigitalService.updateEstadoDocumento(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['expediente_digital', 'archivos', variables.id_expediente],
      });
    },
  });
}
