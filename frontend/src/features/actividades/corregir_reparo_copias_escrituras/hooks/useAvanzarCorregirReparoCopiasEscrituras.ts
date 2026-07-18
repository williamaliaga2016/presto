import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoCopiasEscriturasService } from "../api/corregirReparoCopiasEscriturasService";

export function useAvanzarCorregirReparoCopiasEscrituras() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoCopiasEscriturasService.avanzar(id_expediente),
  });
}
