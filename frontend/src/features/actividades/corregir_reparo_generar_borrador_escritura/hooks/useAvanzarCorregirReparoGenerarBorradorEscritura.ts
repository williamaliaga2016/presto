import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoGenerarBorradorEscrituraService } from "../api/corregirReparoGenerarBorradorEscrituraService";

export function useAvanzarCorregirReparoGenerarBorradorEscritura() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoGenerarBorradorEscrituraService.avanzar(id_expediente),
  });
}
