import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirNotariaReparoAbogados } from '../models/corregir_notaria_reparo_abogados';
import { corregirNotariaReparoAbogadosService } from '../api/corregirNotariaReparoAbogadosService';

export function useUpsertCorregirNotariaReparoAbogados() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirNotariaReparoAbogados>, unknown, CorregirNotariaReparoAbogados>({
    mutationFn: (payload) => corregirNotariaReparoAbogadosService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_notaria_reparo_abogados', variables.id_expediente],
      });
    },
  });
}
