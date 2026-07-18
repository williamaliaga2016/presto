import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoGenerarMemoEscritura } from "../models/corregir_reparo_generar_memo_escritura";
import { corregirReparoGenerarMemoEscrituraService } from "../api/corregirReparoGenerarMemoEscrituraService";

export function useCorregirReparoGenerarMemoEscritura(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoGenerarMemoEscritura | null>>({
    queryKey: ["corregir_reparo_generar_memo_escritura", id_expediente],
    queryFn: () => corregirReparoGenerarMemoEscrituraService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
