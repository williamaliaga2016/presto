import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoEstudioTitulos } from "../models/corregir_reparo_estudio_titulos";
import { corregirReparoEstudioTitulosService } from "../api/corregirReparoEstudioTitulosService";

export function useCorregirReparoEstudioTitulos(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoEstudioTitulos | null>>({
    queryKey: ["corregir_reparo_estudio_titulos", id_expediente],
    queryFn: () => corregirReparoEstudioTitulosService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
