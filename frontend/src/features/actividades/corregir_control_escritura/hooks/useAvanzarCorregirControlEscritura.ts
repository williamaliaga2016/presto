import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirControlEscrituraService } from "../api/corregirControlEscrituraService";

export function useAvanzarCorregirControlEscritura() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirControlEscrituraService.avanzar(id_expediente),
  });
}
