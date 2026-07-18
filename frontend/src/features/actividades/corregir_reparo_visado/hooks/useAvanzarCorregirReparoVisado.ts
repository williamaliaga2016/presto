import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoVisadoService } from "../api/corregirReparoVisadoService";

export function useAvanzarCorregirReparoVisado() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoVisadoService.avanzar(id_expediente),
  });
}
