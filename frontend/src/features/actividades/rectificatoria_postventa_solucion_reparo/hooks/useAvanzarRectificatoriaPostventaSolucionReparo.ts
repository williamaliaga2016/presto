import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { rectificatoriaPostventaSolucionReparoService } from "../api/rectificatoriaPostventaSolucionReparoService";

export function useAvanzarRectificatoriaPostventaSolucionReparo() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => rectificatoriaPostventaSolucionReparoService.avanzar(id_expediente),
  });
}
