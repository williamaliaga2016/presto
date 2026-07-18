import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalFirmaAlzante } from "../models/rectificatoria_legal_firma_alzante";
import { rectificatoriaLegalFirmaAlzanteService } from "../api/rectificatoriaLegalFirmaAlzanteService";

export function useUpsertRectificatoriaLegalFirmaAlzante() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RectificatoriaLegalFirmaAlzante>, unknown, RectificatoriaLegalFirmaAlzante>({
    mutationFn: (payload) => rectificatoriaLegalFirmaAlzanteService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["rectificatoria_legal_firma_alzante", variables.id_expediente],
      });
    },
  });
}
