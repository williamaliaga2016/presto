import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaLegalCierreCopiasPostventa } from "../models/rectificatoria_legal_cierre_copias_postventa";
import { rectificatoriaLegalCierreCopiasPostventaService } from "../api/rectificatoriaLegalCierreCopiasPostventaService";

export function useRectificatoriaLegalCierreCopiasPostventa(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaLegalCierreCopiasPostventa | null>>({
    queryKey: ["rectificatoria_legal_cierre_copias_postventa", id_expediente],
    queryFn: () => rectificatoriaLegalCierreCopiasPostventaService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
