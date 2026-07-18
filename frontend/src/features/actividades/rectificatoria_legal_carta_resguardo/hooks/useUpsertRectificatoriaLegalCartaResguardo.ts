import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCartaResguardo } from "../models/rectificatoria_legal_carta_resguardo";
import { rectificatoriaLegalCartaResguardoService } from "../api/rectificatoriaLegalCartaResguardoService";

export function useUpsertRectificatoriaLegalCartaResguardo() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RectificatoriaLegalCartaResguardo>, unknown, RectificatoriaLegalCartaResguardo>({
    mutationFn: (payload) => rectificatoriaLegalCartaResguardoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["rectificatoria_legal_carta_resguardo", variables.id_expediente],
      });
    },
  });
}
