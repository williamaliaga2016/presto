import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { catalogoService } from '../api/catalogoService';
import type { ControlesAntecedenteCredito } from '../models/catalogo';

export function useControlesAntecedenteCredito(enabled = true) {
  return useQuery<ApiResponse<ControlesAntecedenteCredito>>({
    queryKey: ['carga_operacion_banco_controles_antecedente_credito'],
    queryFn: () => catalogoService.getControlesAntecedenteCredito(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
