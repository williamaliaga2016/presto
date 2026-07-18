import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';
import { asignarEstudioTitulosService } from '../api/asignarEstudioTitulosService';

export function useCatalogoAbogado() {
  return useQuery<ApiResponse<ControlBaseDTO[]>>({
    queryKey: ['asignar_estudio_titulos', 'catalogo_abogado'],
    queryFn: () => asignarEstudioTitulosService.getCatalogoAbogado(),
  });
}

