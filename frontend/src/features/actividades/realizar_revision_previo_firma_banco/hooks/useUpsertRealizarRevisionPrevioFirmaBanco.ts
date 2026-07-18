import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarRevisionPrevioFirmaBanco } from '../models/realizar_revision_previo_firma_banco';
import { realizarRevisionPrevioFirmaBancoService } from '../api/realizarRevisionPrevioFirmaBancoService';

export function useUpsertRealizarRevisionPrevioFirmaBanco() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RealizarRevisionPrevioFirmaBanco>, unknown, RealizarRevisionPrevioFirmaBanco>({
    mutationFn: (payload) => realizarRevisionPrevioFirmaBancoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['realizar_revision_previo_firma_banco', variables.id_expediente],
      });
    },
  });
}
