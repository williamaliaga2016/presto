import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCierreCopiasPostventa } from "../models/rectificatoria_legal_cierre_copias_postventa";
import { rectificatoriaLegalCierreCopiasPostventaService } from "../api/rectificatoriaLegalCierreCopiasPostventaService";

export function useUpsertRectificatoriaLegalCierreCopiasPostventa() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RectificatoriaLegalCierreCopiasPostventa>, unknown, RectificatoriaLegalCierreCopiasPostventa>({
    mutationFn: (payload) => rectificatoriaLegalCierreCopiasPostventaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["rectificatoria_legal_cierre_copias_postventa", variables.id_expediente],
      });
    },
  });
}
