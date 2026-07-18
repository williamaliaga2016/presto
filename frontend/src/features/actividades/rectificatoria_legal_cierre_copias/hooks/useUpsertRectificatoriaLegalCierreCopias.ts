import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCierreCopias } from "../models/rectificatoria_legal_cierre_copias";
import { rectificatoriaLegalCierreCopiasService } from "../api/rectificatoriaLegalCierreCopiasService";

export function useUpsertRectificatoriaLegalCierreCopias() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RectificatoriaLegalCierreCopias>, unknown, RectificatoriaLegalCierreCopias>({
    mutationFn: (payload) => rectificatoriaLegalCierreCopiasService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["rectificatoria_legal_cierre_copias", variables.id_expediente],
      });
    },
  });
}
