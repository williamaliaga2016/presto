import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { revisarIngresoDatosCreditoService } from '../api/revisarIngresoDatosCreditoService';
import type { ControlesDatosCredito } from '../models/catalogo';

export function useControlesDatosCredito(enabled = true) {
  return useQuery<ApiResponse<ControlesDatosCredito>>({
    queryKey: ['datos_operacion_controles_datos_credito'],
    queryFn: () => revisarIngresoDatosCreditoService.getControlesDatosCredito(),
    enabled,
  });
}
