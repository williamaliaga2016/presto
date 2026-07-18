import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RectificatoriaPostventaSolucionReparo } from "../models/rectificatoria_postventa_solucion_reparo";
import { rectificatoriaPostventaSolucionReparoService } from "../api/rectificatoriaPostventaSolucionReparoService";

export function useRectificatoriaPostventaSolucionReparo(id_expediente: number) {
  return useQuery<ApiResponse<RectificatoriaPostventaSolucionReparo | null>>({
    queryKey: ["corregir_reparo_generar_memo_escritura", id_expediente],
    queryFn: () => rectificatoriaPostventaSolucionReparoService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
