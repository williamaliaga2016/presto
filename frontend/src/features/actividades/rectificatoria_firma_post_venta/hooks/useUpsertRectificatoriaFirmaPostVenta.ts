import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirmaPostVenta } from "../models/rectificatoria_firma_post_venta";
import { rectificatoriaFirmaPostVentaService } from "../api/rectificatoriaFirmaPostVentaService";

export function useUpsertRectificatoriaFirmaPostVenta() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<RectificatoriaFirmaPostVenta>, unknown, RectificatoriaFirmaPostVenta>({
    mutationFn: (payload) => rectificatoriaFirmaPostVentaService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["rectificatoria_firma_post_venta", variables.id_expediente],
      });
    },
  });
}
