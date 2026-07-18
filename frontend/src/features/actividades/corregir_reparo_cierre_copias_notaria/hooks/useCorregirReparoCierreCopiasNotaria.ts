import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCierreCopiasNotaria } from "../models/corregir_reparo_cierre_copias_notaria";
import { corregirReparoCierreCopiasNotariaService } from "../api/corregirReparoCierreCopiasNotariaService";

export function useCorregirReparoCierreCopiasNotaria(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoCierreCopiasNotaria | null>>({
    queryKey: ['corregir_reparo_cierre_copias_notaria', id_expediente],
    queryFn: () => corregirReparoCierreCopiasNotariaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}