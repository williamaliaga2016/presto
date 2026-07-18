import { useQuery } from "@tanstack/react-query";
import type { ApiResponse } from "@/core/api/models/ApiResponse";
import type { VerificarReparoDatosOperacion } from "../models/verificar_reparo_datos_operacion";
import { verificarReparoIngresarDatosOperacionService } from "../api/verificarReparoIngresarDatosOperacionService";

export function useVerificarReparoIngresarDatosOperacion(id_expediente: number) {
  return useQuery<ApiResponse<VerificarReparoDatosOperacion | null>>({
    queryKey: ["verificar_reparo_ingresar_datos_operacion", id_expediente],
    queryFn: () =>
      verificarReparoIngresarDatosOperacionService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
