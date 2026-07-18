import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoDatosOperacion } from "../models/corregir_reparo_datos_operacion";
import { corregirReparoDatosOperacionService } from "../api/corregirReparoDatosOperacionService";

export function useCorregirReparoDatosOperacion(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoDatosOperacion | null>>({
    queryKey: ["corregir_reparo_datos_operacion", id_expediente],
    queryFn: () => corregirReparoDatosOperacionService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
