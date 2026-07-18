import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { EvaluarReparoAutomatico } from "../models/registrar_tasacion";
import { registrarTasacionService } from "../api/registrarTasacionService";

export function useEvaluarReparoAutomatico() {
  return useMutation<ApiResponse<EvaluarReparoAutomatico>, unknown, number>({
    mutationFn: (id_expediente) =>
      registrarTasacionService.evaluarReparoAutomatico(id_expediente),
  });
}
