import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { rectificatoriaLegalCierreCopiasPostventaService } from "../api/rectificatoriaLegalCierreCopiasPostventaService";

export function useAvanzarRectificatoriaLegalCierreCopiasPostventa() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaLegalCierreCopiasPostventaService.avanzar(id_expediente),
  });
}
