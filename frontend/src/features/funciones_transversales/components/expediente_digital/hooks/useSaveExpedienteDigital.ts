import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';
import type {
  ExpedienteDigital,
  ExpedienteDigitalSavePayload,
} from '../models/ExpedienteDigital';

export function useSaveExpedienteDigital() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<ExpedienteDigital>,
    unknown,
    ExpedienteDigitalSavePayload
  >({
    mutationFn: (payload) => expedienteDigitalService.saveExpedienteDigital(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['expediente_digital', 'archivos', variables.id_expediente],
      });
      queryClient.invalidateQueries({
        queryKey: ['expediente_digital', 'documentos', variables.id_expediente],
      });
    },
  });
}
