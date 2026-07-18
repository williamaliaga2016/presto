import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoGenerarMemoEscritura } from "../models/corregir_reparo_generar_memo_escritura";
import { corregirReparoGenerarMemoEscrituraService } from "../api/corregirReparoGenerarMemoEscrituraService";

export function useUpsertCorregirReparoGenerarMemoEscritura() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<CorregirReparoGenerarMemoEscritura>,
    unknown,
    CorregirReparoGenerarMemoEscritura
  >({
    mutationFn: (payload) => corregirReparoGenerarMemoEscrituraService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_reparo_generar_memo_escritura", variables.id_expediente],
      });
    },
  });
}
