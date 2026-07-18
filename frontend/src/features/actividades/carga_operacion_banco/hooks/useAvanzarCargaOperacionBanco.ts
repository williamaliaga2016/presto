import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { cargaOperacionBancoService } from '../api/cargaOperacionBancoService';

export function useAvanzarCargaOperacionBanco() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => cargaOperacionBancoService.avanzar(id_expediente),
  });
}
