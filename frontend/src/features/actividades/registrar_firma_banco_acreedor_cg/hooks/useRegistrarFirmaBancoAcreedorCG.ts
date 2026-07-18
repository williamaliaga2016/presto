import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RegistrarFirmaBancoAcreedorCG } from "../models/registrar_firma_banco_acreedor_cg";
import { registrarFirmaBancoAcreedorCGService } from "../api/registrarFirmaBancoAcreedorCGService";

export function useRegistrarFirmaBancoAcreedorCG(id_expediente: number) {
  return useQuery<ApiResponse<RegistrarFirmaBancoAcreedorCG | null>>({
    queryKey: ["registrar_firma_banco_acreedor_cg", id_expediente],
    queryFn: () =>
      registrarFirmaBancoAcreedorCGService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
