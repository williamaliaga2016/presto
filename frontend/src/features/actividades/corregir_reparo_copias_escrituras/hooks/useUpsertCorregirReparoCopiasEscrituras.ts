import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCopiasEscrituras } from "../models/corregir_reparo_copias_escrituras";
import { corregirReparoCopiasEscriturasService } from "../api/corregirReparoCopiasEscriturasService";

export function useUpsertCorregirReparoCopiasEscrituras() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoCopiasEscrituras>, unknown, CorregirReparoCopiasEscrituras>({
    mutationFn: (payload) => corregirReparoCopiasEscriturasService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_reparo_copias_escrituras", variables.id_expediente],
      });
    },
  });
}
