import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { rectificatoriaLegalCartaResguardoService } from "../api/rectificatoriaLegalCartaResguardoService";

export function useAvanzarRectificatoriaLegalCartaResguardo() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaLegalCartaResguardoService.avanzar(id_expediente),
  });
}
