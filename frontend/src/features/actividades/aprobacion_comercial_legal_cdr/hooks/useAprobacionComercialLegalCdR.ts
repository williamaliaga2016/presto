import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { AprobacionComercialLegalCdR } from "../models/aprobacion_comercial_legal_cdr";
import { aprobacionComercialLegalCdRService } from "../api/aprobacionComercialLegalCdRService";

export function useAprobacionComercialLegalCdR(id_expediente: number) {
  return useQuery<ApiResponse<AprobacionComercialLegalCdR | null>>({
    queryKey: ["aprobacion_comercial_legal_cdr", id_expediente],
    queryFn: () => aprobacionComercialLegalCdRService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
