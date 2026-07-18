import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesAntecedenteComprador } from '../models/catalogo';

export function useControlesAntecedenteComprador(enabled = true) {
  return useQuery<ApiResponse<ControlesAntecedenteComprador>>({
    queryKey: ['carga_operacion_banco_controles_antecedente_comprador'],
    queryFn: () => catalogoService.getControlesAntecedenteComprador(),
    enabled,
    staleTime: 1000 * 60 * 10,
    retry: 1,
  });
}
