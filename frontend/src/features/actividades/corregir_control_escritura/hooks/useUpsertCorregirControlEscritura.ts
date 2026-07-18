import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirControlEscritura } from "../models/corregir_control_escritura";
import { corregirControlEscrituraService } from "../api/corregirControlEscrituraService";

export function useUpsertCorregirControlEscritura() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirControlEscritura>, unknown, CorregirControlEscritura>({
    mutationFn: (payload) => corregirControlEscrituraService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_control_escritura", variables.id_expediente],
      });
    },
  });
}
