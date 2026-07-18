import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesDatosOperacion } from '../models/catalogo';

export function useControlesDatosOperacion(enabled = true) {
  return useQuery<ApiResponse<ControlesDatosOperacion>>({
    queryKey: ['carga_operacion_banco_controles_datos_operacion'],
    queryFn: () => catalogoService.getControlesDatosOperacion(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
