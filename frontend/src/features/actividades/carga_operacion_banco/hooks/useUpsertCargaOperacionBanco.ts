import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CargaOperacionBanco } from '../models/carga_operacion_banco';
import { cargaOperacionBancoService } from '../api/cargaOperacionBancoService';

export interface UpsertCargaOperacionBancoVariables {
  payload: CargaOperacionBanco;
}

export function useUpsertCargaOperacionBanco() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CargaOperacionBanco>, unknown, UpsertCargaOperacionBancoVariables>({
    mutationFn: ({ payload }) => cargaOperacionBancoService.save(payload),
    onSuccess: (_, { payload }) => {
      queryClient.invalidateQueries({
        queryKey: ['carga_operacion_banco', payload.id_expediente],
      });
    },
  });
}
