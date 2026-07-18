import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoEstudioTitulosService } from "../api/corregirReparoEstudioTitulosService";

export function useAvanzarCorregirReparoEstudioTitulos() {
  return useMutation<ApiResponse<unknown>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoEstudioTitulosService.avanzar(id_expediente),
  });
}
