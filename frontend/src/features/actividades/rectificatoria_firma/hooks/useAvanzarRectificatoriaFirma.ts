import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { rectificatoriaFirmaService } from "../api/rectificatoriaFirmaService";

export function useAvanzarRectificatoriaFirma() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaFirmaService.avanzar(id_expediente),
  });
}
