import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCdr } from "../models/corregir_reparo_cdr";
import { corregirReparoCdrService } from "../api/corregirReparoCdrService";

export function useUpsertCorregirReparoCdr() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<CorregirReparoCdr>, unknown, CorregirReparoCdr>({
    mutationFn: (payload) => corregirReparoCdrService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["corregir_reparo_cdr", variables.id_expediente],
      });
    },
  });
}
