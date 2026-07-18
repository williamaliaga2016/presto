import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirCartaResguardo } from "../models/corregir_carta_resguardo";
import { corregirCartaResguardoService } from "../api/corregirCartaResguardoService";

export function useCorregirCartaResguardo(id_expediente: number) {
  return useQuery<ApiResponse<CorregirCartaResguardo | null>>({
    queryKey: ["corregir_carta_resguardo", id_expediente],
    queryFn: () => corregirCartaResguardoService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
