import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { corregirReparoCalculoDocService } from "../api/corregirReparoCalculoDocService";

export function useAvanzarCorregirReparoCalculoDoc() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirReparoCalculoDocService.avanzar(id_expediente),
  });
}
