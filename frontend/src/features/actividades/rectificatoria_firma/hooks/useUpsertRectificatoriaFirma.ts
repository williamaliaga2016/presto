import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirma } from "../models/rectificatoria_firma";
import { rectificatoriaFirmaService } from "../api/rectificatoriaFirmaService";

export function useUpsertRectificatoriaFirma() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RectificatoriaFirma>, unknown, RectificatoriaFirma>({
    mutationFn: (payload) => rectificatoriaFirmaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["rectificatoria_firma", variables.id_expediente],
      });
    },
  });
}
