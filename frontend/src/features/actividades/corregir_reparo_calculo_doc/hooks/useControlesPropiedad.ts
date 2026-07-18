import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoCalculoDocService } from "../api/corregirReparoCalculoDocService";
import type { ControlesPropiedad } from "../models/catalogo";

export function useControlesPropiedad(enabled = true) {
  return useQuery<ApiResponse<ControlesPropiedad>>({
    queryKey: ["corregir_reparo_calculo_doc_controles_propiedad"],
    queryFn: () => corregirReparoCalculoDocService.getControlesPropiedad(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
