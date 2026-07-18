import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { VerificarReparoDatosOperacion } from "../models/verificar_reparo_datos_operacion";
import { verificarReparoIngresarDatosOperacionService } from "../api/verificarReparoIngresarDatosOperacionService";

export function useUpsertVerificarReparoIngresarDatosOperacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<VerificarReparoDatosOperacion>, unknown, VerificarReparoDatosOperacion>({
    mutationFn: (payload) => verificarReparoIngresarDatosOperacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["verificar_reparo_ingresar_datos_operacion", variables.id_expediente],
      });
    },
  });
}
