import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCierreCopiasNotaria } from "../models/corregir_reparo_cierre_copias_notaria";
import { corregirReparoCierreCopiasNotariaService } from "../api/corregirReparoCierreCopiasNotariaService";


export function useUpsertCorregirReparoCierreCopiasNotaria() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoCierreCopiasNotaria>, unknown, CorregirReparoCierreCopiasNotaria>({
    mutationFn: (payload) => corregirReparoCierreCopiasNotariaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_cierre_copias_notaria', variables.id_expediente],
      });
    },
  });
}
