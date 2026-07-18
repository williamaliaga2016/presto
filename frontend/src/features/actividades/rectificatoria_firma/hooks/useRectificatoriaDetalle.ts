import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirma } from "../models/rectificatoria_firma";
import { rectificatoriaFirmaService } from "../api/rectificatoriaFirmaService";

export function useRectificatoriaDetalle(
  id_expediente: number,
  rol_comparecencia: string,
) {
  return useQuery({
    queryKey: [
      "rectificatoria-detalle",
      id_expediente,
      rol_comparecencia,
    ],
    queryFn: () =>
      rectificatoriaFirmaService.getDetalleByExpediente(
        id_expediente,
        rol_comparecencia,
      ),
    enabled: !!id_expediente,
  });
}
