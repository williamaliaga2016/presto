import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoCdrService } from "../api/corregirReparoCdrService";

export function useAvanzarCorregirReparoCdr() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoCdrService.avanzar(id_expediente),
  });
}
