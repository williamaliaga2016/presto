import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { calculoGeneracionDocumentoService } from '../api/calculoGeneracionDocumentoService';

export function useCalcularUF(fecha: string | null) {
  return useQuery<ApiResponse<number>>({
    queryKey: ['calculo_uf', fecha],
    queryFn: () => calculoGeneracionDocumentoService.calcularUF(fecha!),
    enabled: !!fecha,
    staleTime: 1000 * 60 * 5,
  });
}
