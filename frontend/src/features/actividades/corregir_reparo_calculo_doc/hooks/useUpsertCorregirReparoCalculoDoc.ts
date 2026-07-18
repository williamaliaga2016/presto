import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCalculoDoc } from "../models/corregir_reparo_calculo_doc";
import { corregirReparoCalculoDocService } from "../api/corregirReparoCalculoDocService";

export function useUpsertCorregirReparoCalculoDoc() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoCalculoDoc>, unknown, CorregirReparoCalculoDoc>({
    mutationFn: (payload) => corregirReparoCalculoDocService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_reparo_calculo_doc", variables.id_expediente],
      });
    },
  });
}
