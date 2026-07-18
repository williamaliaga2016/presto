import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaFirma } from "../models/rectificatoria_firma";
import { rectificatoriaFirmaService } from "../api/rectificatoriaFirmaService";

export function useRectificatoriaFirma(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaFirma | null>>({
    queryKey: ["rectificatoria_firma", id_expediente],
    queryFn: () => rectificatoriaFirmaService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
