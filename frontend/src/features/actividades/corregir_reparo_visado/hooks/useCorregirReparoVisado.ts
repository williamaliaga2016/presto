import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoVisado } from "../models/corregir_reparo_visado";
import { corregirReparoVisadoService } from "../api/corregirReparoVisadoService";

export function useCorregirReparoVisado(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoVisado | null>>({
    queryKey: ["corregir_reparo_visado", id_expediente],
    queryFn: () => corregirReparoVisadoService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
