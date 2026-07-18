import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { rectificatoriaLegalFirmaAlzanteService } from "../api/rectificatoriaLegalFirmaAlzanteService";

export function useAvanzarRectificatoriaLegalFirmaAlzante() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaLegalFirmaAlzanteService.avanzar(id_expediente),
  });
}
