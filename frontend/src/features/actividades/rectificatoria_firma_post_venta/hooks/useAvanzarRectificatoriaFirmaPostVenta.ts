import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { rectificatoriaFirmaPostVentaService } from "../api/rectificatoriaFirmaPostVentaService";

export function useAvanzarRectificatoriaFirmaPostVenta() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaFirmaPostVentaService.avanzar(id_expediente),
  });
}
