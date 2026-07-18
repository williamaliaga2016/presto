import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { ReparoFormulario } from "../models/reparo_formulario";
import { reparoFormularioService } from "../api/reparoFormularioService";

export function useReparoFormulario(id_expediente: number) {
  return useQuery<ApiResponse<ReparoFormulario | null>>({
    queryKey: ["reparo_formulario", id_expediente],
    queryFn: () => reparoFormularioService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
