import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { aprobacionComercialLegalCdRService } from "../api/aprobacionComercialLegalCdRService";

export function useAvanzarAprobacionComercialLegalCdR() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => aprobacionComercialLegalCdRService.avanzar(id_expediente),
  });
}
