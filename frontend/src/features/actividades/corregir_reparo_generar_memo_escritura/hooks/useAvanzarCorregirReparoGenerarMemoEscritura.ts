import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoGenerarMemoEscrituraService } from "../api/corregirReparoGenerarMemoEscrituraService";

export function useAvanzarCorregirReparoGenerarMemoEscritura() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoGenerarMemoEscrituraService.avanzar(id_expediente),
  });
}
