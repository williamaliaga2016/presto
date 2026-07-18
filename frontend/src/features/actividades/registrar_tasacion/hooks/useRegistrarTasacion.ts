import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { Tasacion } from "../models/registrar_tasacion";
import { registrarTasacionService } from "../api/registrarTasacionService";

export function useRegistrarTasacion(id_expediente: number) {
  return useQuery<ApiResponse<Tasacion | null>>({
    queryKey: ["registrar_tasacion", id_expediente],
    queryFn: () => registrarTasacionService.getByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
