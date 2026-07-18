import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirCartaResguardoService } from "../api/corregirCartaResguardoService";

export function useAvanzarCorregirCartaResguardo() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirCartaResguardoService.avanzar(id_expediente),
  });
}
