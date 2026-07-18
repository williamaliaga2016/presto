import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoDatosOperacionService } from "../api/corregirReparoDatosOperacionService";

export function useAvanzarCorregirReparoDatosOperacion() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoDatosOperacionService.avanzar(id_expediente),
  });
}
