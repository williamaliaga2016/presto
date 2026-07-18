import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { Tasacion } from "../models/registrar_tasacion";
import { registrarTasacionService } from "../api/registrarTasacionService";

export function useUpsertRegistrarTasacion() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<Tasacion>, unknown, Tasacion>({
    mutationFn: (payload) => registrarTasacionService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["registrar_tasacion", variables.id_expediente],
      });
      queryClient.invalidateQueries({
        queryKey: ["registrar_tasacion_detalles", variables.id_expediente],
      });
    },
  });
}
