import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { AprobacionComercialLegalCdR } from "../models/aprobacion_comercial_legal_cdr";
import { aprobacionComercialLegalCdRService } from "../api/aprobacionComercialLegalCdRService";

export function useUpsertAprobacionComercialLegalCdR() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<AprobacionComercialLegalCdR>, unknown, AprobacionComercialLegalCdR>({
    mutationFn: (payload) => aprobacionComercialLegalCdRService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["aprobacion_comercial_legal_cdr", variables.id_expediente],
      });
    },
  });
}
