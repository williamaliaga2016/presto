import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirmaPostVenta } from "../models/rectificatoria_firma_post_venta";
import { rectificatoriaFirmaPostVentaService } from "../api/rectificatoriaFirmaPostVentaService";

export function useRectificatoriaPostVentaDetalle(
  id_expediente: number,
  rol_comparecencia: string,
) {
  return useQuery({
    queryKey: [
      "rectificatoria_post_venta-detalle",
      id_expediente,
      rol_comparecencia,
    ],
    queryFn: () =>
      rectificatoriaFirmaPostVentaService.getDetalleByExpediente(
        id_expediente,
        rol_comparecencia,
      ),
    enabled: !!id_expediente,
  });
}
