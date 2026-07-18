import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoGenerarBorradorEscritura } from "../models/corregir_reparo_generar_borrador_escritura";
import { corregirReparoGenerarBorradorEscrituraService } from "../api/corregirReparoGenerarBorradorEscrituraService";

export function useCorregirReparoGenerarBorradorEscritura(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoGenerarBorradorEscritura | null>>({
    queryKey: ["corregir_reparo_generar_borrador_escritura", id_expediente],
    queryFn: () => corregirReparoGenerarBorradorEscrituraService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
