import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoGenerarBorradorEscritura } from "../models/corregir_reparo_generar_borrador_escritura";
import { corregirReparoGenerarBorradorEscrituraService } from "../api/corregirReparoGenerarBorradorEscrituraService";

export function useUpsertCorregirReparoGenerarBorradorEscritura() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoGenerarBorradorEscritura>, unknown, CorregirReparoGenerarBorradorEscritura>({
    mutationFn: (payload) => corregirReparoGenerarBorradorEscrituraService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_reparo_generar_borrador_escritura", variables.id_expediente],
      });
    },
  });
}
