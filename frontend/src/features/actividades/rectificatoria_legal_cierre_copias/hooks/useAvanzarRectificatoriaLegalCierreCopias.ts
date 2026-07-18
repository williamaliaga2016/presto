import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { rectificatoriaLegalCierreCopiasService } from "../api/rectificatoriaLegalCierreCopiasService";

export function useAvanzarRectificatoriaLegalCierreCopias() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaLegalCierreCopiasService.avanzar(id_expediente),
  });
}
