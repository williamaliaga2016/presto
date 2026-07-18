import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { RegistrarFirmaBancoAcreedorCG } from "../models/registrar_firma_banco_acreedor_cg";
import { registrarFirmaBancoAcreedorCGService } from "../api/registrarFirmaBancoAcreedorCGService";

export function useUpsertRegistrarFirmaBancoAcreedorCG() {
  const queryClient = useQueryClient();
  return useMutation<
    ApiResponse<RegistrarFirmaBancoAcreedorCG>,
    unknown,
    RegistrarFirmaBancoAcreedorCG
  >({
    mutationFn: (payload) => registrarFirmaBancoAcreedorCGService.save(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: [
          "registrar_firma_banco_acreedor_cg",
          variables.id_expediente,
        ],
      });
    },
  });
}
