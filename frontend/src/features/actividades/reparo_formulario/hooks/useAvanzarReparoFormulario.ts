import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { reparoFormularioService } from "../api/reparoFormularioService";

export function useAvanzarReparoFormulario() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => reparoFormularioService.avanzar(id_expediente),
  });
}
