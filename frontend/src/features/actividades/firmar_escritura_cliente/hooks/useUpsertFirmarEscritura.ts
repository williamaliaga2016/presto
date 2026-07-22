import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmarEscrituraCliente } from '../models/firmar_escritura_cliente';
import { firmarEscrituraClienteService } from '../api/firmarEscrituraClienteService';

export function useUpsertFirmarEscritura() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<FirmarEscrituraCliente>, unknown, FirmarEscrituraCliente>({
    mutationFn: (payload) => firmarEscrituraClienteService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['firmar_escritura_cliente', variables.id_expediente],
      });
    },
  });
}
