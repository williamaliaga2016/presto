import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoEstudioTitulos } from "../models/corregir_reparo_estudio_titulos";
import { corregirReparoEstudioTitulosService } from "../api/corregirReparoEstudioTitulosService";

export function useUpsertCorregirReparoEstudioTitulos() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoEstudioTitulos>, unknown, CorregirReparoEstudioTitulos>({
    mutationFn: (payload) => corregirReparoEstudioTitulosService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_reparo_estudio_titulos", variables.id_expediente],
      });
    },
  });
}
