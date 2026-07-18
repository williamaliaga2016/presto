import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { calculoGeneracionDocumentoService } from "../api/calculoGeneracionDocumentoService";
import type { ControlesPropiedad } from "../models/catalogo";

export function useControlesPropiedad(enabled = true) {
  return useQuery<ApiResponse<ControlesPropiedad>>({
    queryKey: ["calculo_generacion_documento_controles_propiedad"],
    queryFn: () => calculoGeneracionDocumentoService.getControlesPropiedad(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
