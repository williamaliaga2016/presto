import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCdr } from "../models/corregir_reparo_cdr";
import { corregirReparoCdrService } from "../api/corregirReparoCdrService";

export function useCorregirReparoCdr(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoCdr | null>>({
    queryKey: ["corregir_reparo_cdr", id_expediente],
    queryFn: () => corregirReparoCdrService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
