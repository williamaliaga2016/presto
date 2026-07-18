import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarDatosOperacionService } from '../api/revisarDatosOperacionService';
import type { RevisarDatosOperacionVendedor } from '../models/revisar_datos_operacion';

export function useUpsertVendedor() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<RevisarDatosOperacionVendedor>,
    unknown,
    RevisarDatosOperacionVendedor
  >({
    mutationFn: (payload) => revisarDatosOperacionService.saveVendedor(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_datos_operacion', variables.id_expediente],
      });
    },
  });
}
