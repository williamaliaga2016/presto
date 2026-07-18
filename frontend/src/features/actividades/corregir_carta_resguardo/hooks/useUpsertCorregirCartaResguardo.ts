import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirCartaResguardo } from "../models/corregir_carta_resguardo";
import { corregirCartaResguardoService } from "../api/corregirCartaResguardoService";

export function useUpsertCorregirCartaResguardo() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirCartaResguardo>, unknown, CorregirCartaResguardo>({
    mutationFn: (payload) => corregirCartaResguardoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_carta_resguardo", variables.id_expediente],
      });
    },
  });
}
