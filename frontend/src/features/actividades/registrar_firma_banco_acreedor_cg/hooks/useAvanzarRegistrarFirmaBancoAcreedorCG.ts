import { useMutation } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import { registrarFirmaBancoAcreedorCGService } from "../api/registrarFirmaBancoAcreedorCGService";

export function useAvanzarRegistrarFirmaBancoAcreedorCG() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) =>
      registrarFirmaBancoAcreedorCGService.avanzar(id_expediente),
  });
}
