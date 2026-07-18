import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { verificarReparoIngresarDatosOperacionService } from "../api/verificarReparoIngresarDatosOperacionService";

export function useAvanzarVerificarReparoIngresarDatosOperacion() {
  return useMutation<ApiResponse<unknown>, unknown, number>({
    mutationFn: (id_expediente) =>
      verificarReparoIngresarDatosOperacionService.avanzar(id_expediente),
  });
}
