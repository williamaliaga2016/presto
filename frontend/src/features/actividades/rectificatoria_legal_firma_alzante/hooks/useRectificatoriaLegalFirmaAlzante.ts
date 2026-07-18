import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalFirmaAlzante } from "../models/rectificatoria_legal_firma_alzante";
import { rectificatoriaLegalFirmaAlzanteService } from "../api/rectificatoriaLegalFirmaAlzanteService";

export function useRectificatoriaLegalFirmaAlzante(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaLegalFirmaAlzante | null>>({
    queryKey: ["rectificatoria_legal_firma_alzante", id_expediente],
    queryFn: () => rectificatoriaLegalFirmaAlzanteService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
