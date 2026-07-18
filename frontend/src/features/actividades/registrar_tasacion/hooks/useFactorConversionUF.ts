import { useQuery } from "@tanstack/react-query";
import { cargaOperacionBancoService } from "@/features/actividades/carga_operacion_banco/api/cargaOperacionBancoService";

export function useFactorConversionUF(id_expediente: number) {
  return useQuery({
    queryKey: ["factor_conversion_uf", id_expediente],
    queryFn: async () => {
      const response =
        await cargaOperacionBancoService.getAntecedenteCreditoByExpediente(
          id_expediente,
        );
      return response.detail?.factor_conversion_uf ?? null;
    },
    enabled: !!id_expediente && id_expediente > 0,
  });
}
