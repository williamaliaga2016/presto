import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoDatosOperacion } from "../models/corregir_reparo_datos_operacion";
import { corregirReparoDatosOperacionService } from "../api/corregirReparoDatosOperacionService";

export function useUpsertCorregirReparoDatosOperacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoDatosOperacion>, unknown, CorregirReparoDatosOperacion>({
    mutationFn: (payload) => corregirReparoDatosOperacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['corregir_reparo_datos_operacion', variables.id_expediente],
      });
    },
  });
}
