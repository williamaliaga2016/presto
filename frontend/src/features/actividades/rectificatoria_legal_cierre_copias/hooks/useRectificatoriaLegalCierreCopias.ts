import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCierreCopias } from "../models/rectificatoria_legal_cierre_copias";
import { rectificatoriaLegalCierreCopiasService } from "../api/rectificatoriaLegalCierreCopiasService";

export function useRectificatoriaLegalCierreCopias(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaLegalCierreCopias | null>>({
    queryKey: ["rectificatoria_legal_cierre_copias", id_expediente],
    queryFn: () => rectificatoriaLegalCierreCopiasService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
