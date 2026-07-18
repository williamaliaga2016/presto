import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { ReparoFormulario } from "../models/reparo_formulario";
import { reparoFormularioService } from "../api/reparoFormularioService";

export function useUpsertReparoFormulario() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ReparoFormulario>, unknown, ReparoFormulario>({
    mutationFn: (payload) => reparoFormularioService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ["reparo_formulario", variables.id_expediente],
      });
    },
  });
}
