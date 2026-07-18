import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { TasacionDetalle } from "../models/tasacion_detalle";
import { registrarTasacionService } from "../api/registrarTasacionService";

export function useTasacionDetalles(id_expediente: number) {
  return useQuery<ApiResponse<TasacionDetalle[]>>({
    queryKey: ["registrar_tasacion_detalles", id_expediente],
    queryFn: () => registrarTasacionService.getDetallesByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
