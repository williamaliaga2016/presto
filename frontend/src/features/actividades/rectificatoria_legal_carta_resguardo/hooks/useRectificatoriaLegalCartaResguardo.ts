import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCartaResguardo } from "../models/rectificatoria_legal_carta_resguardo";
import { rectificatoriaLegalCartaResguardoService } from "../api/rectificatoriaLegalCartaResguardoService";

export function useRectificatoriaLegalCartaResguardo(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaLegalCartaResguardo | null>>({
    queryKey: ["rectificatoria_legal_carta_resguardo", id_expediente],
    queryFn: () => rectificatoriaLegalCartaResguardoService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
