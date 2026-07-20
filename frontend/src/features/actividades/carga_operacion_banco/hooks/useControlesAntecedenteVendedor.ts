import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesAntecedenteVendedor } from '../models/catalogo';

export function useControlesAntecedenteVendedor(enabled = true) {
  return useQuery<ApiResponse<ControlesAntecedenteVendedor>>({
    queryKey: ['carga_operacion_banco_controles_antecedente_vendedor'],
    queryFn: () => catalogoService.getControlesAntecedenteVendedor(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
