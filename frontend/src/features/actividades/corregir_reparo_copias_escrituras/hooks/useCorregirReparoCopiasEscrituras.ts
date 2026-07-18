import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCopiasEscrituras } from "../models/corregir_reparo_copias_escrituras";
import { corregirReparoCopiasEscriturasService } from "../api/corregirReparoCopiasEscriturasService";

export function useCorregirReparoCopiasEscrituras(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoCopiasEscrituras | null>>({
    queryKey: ["corregir_reparo_copias_escrituras", id_expediente],
    queryFn: () => corregirReparoCopiasEscriturasService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
