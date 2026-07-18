import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoVisado } from "../models/corregir_reparo_visado";
import { corregirReparoVisadoService } from "../api/corregirReparoVisadoService";

export function useUpsertCorregirReparoVisado() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoVisado>, unknown, CorregirReparoVisado>({
    mutationFn: (payload) => corregirReparoVisadoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_reparo_visado", variables.id_expediente],
      });
    },
  });
}
