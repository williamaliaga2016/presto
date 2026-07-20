import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesDatosComercial } from '../models/catalogo';

export function useControlesDatosComercial(enabled = true) {
  return useQuery<ApiResponse<ControlesDatosComercial>>({
    queryKey: ['carga_operacion_banco_controles_datos_comercial'],
    queryFn: () => catalogoService.getControlesDatosComercial(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
