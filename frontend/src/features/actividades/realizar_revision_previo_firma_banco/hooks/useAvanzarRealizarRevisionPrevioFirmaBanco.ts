import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { realizarRevisionPrevioFirmaBancoService } from '../api/realizarRevisionPrevioFirmaBancoService';

export function useAvanzarRealizarRevisionPrevioFirmaBanco() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => realizarRevisionPrevioFirmaBancoService.avanzar(id_expediente),
  });
}
