import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaPostventaSolucionReparo } from "../models/rectificatoria_postventa_solucion_reparo";
import { rectificatoriaPostventaSolucionReparoService } from "../api/rectificatoriaPostventaSolucionReparoService";

export function useUpsertRectificatoriaPostventaSolucionReparo() {
  const queryClient = useQueryClient();

  return useMutation<
    ApiResponse<RectificatoriaPostventaSolucionReparo>,
    unknown,
    RectificatoriaPostventaSolucionReparo
  >({
    mutationFn: (payload) => rectificatoriaPostventaSolucionReparoService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["rectificatoria_postventa_solucion_reparo", variables.id_expediente],
      });
    },
  });
}
