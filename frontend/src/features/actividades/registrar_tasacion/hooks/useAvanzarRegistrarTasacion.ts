import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { registrarTasacionService } from "../api/registrarTasacionService";

export function useAvanzarRegistrarTasacion() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) =>
      registrarTasacionService.avanzar(id_expediente),
  });
}
