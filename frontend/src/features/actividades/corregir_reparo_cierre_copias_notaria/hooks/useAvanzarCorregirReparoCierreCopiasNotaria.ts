import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoCierreCopiasNotariaService } from "../api/corregirReparoCierreCopiasNotariaService";

export function useAvanzarCorregirReparoCierreCopiasNotaria() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoCierreCopiasNotariaService.avanzar(id_expediente),
  });
}