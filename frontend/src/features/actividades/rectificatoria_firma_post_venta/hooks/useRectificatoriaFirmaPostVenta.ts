import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirmaPostVenta } from "../models/rectificatoria_firma_post_venta";
import { rectificatoriaFirmaPostVentaService } from "../api/rectificatoriaFirmaPostVentaService";

export function useRectificatoriaFirmaPostVenta(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaFirmaPostVenta | null>>({
    queryKey: ["rectificatoria_firma_post_venta", id_expediente],
    queryFn: () => rectificatoriaFirmaPostVentaService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
