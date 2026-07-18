import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { CorregirReparoCalculoDoc } from "../models/corregir_reparo_calculo_doc";
import { corregirReparoCalculoDocService } from "../api/corregirReparoCalculoDocService";

export function useCorregirReparoCalculoDoc(id_expediente: number) {
  return useQuery<ApiResponse<CorregirReparoCalculoDoc | null>>({
    queryKey: ["corregir_reparo_calculo_doc", id_expediente],
    queryFn: () => corregirReparoCalculoDocService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
