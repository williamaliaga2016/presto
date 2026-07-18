import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirControlEscritura } from "../models/corregir_control_escritura";
import { corregirControlEscrituraService } from "../api/corregirControlEscrituraService";

export function useCorregirControlEscritura(id_expediente: number) {
  return useQuery<ApiResponse<CorregirControlEscritura | null>>({
    queryKey: ["corregir_control_escritura", id_expediente],
    queryFn: () => corregirControlEscrituraService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
