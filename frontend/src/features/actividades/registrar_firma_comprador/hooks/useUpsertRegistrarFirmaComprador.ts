import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmaComprador } from '../models/registrar_firma_comprador';
import type { FirmaCompradorDetalle } from '../models/registrar_firma_comprador_detalle';
import { registrarFirmaCompradorService } from '../api/registrarFirmaCompradorService';

export interface FirmaCompradorRequest extends FirmaComprador {
  firma_comprador_detalle: FirmaCompradorDetalle[];
}

export function useUpsertRegistrarFirmaComprador() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<FirmaCompradorRequest>,
    unknown,
    FirmaCompradorRequest
  >({
    mutationFn: (payload) =>
      registrarFirmaCompradorService.save(payload),

    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['firma_comprador', variables.id_expediente],
      });
    },
  });
}