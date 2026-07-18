import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { AsignarEstudioTitulos } from '../models/asignar_estudio_titulos';
import { asignarEstudioTitulosService } from '../api/asignarEstudioTitulosService';

export function useAsignarEstudioTitulos(id_expediente: number) {
  return useQuery<ApiResponse<AsignarEstudioTitulos | null>>({
    queryKey: ['asignar_estudio_titulos', id_expediente],
    queryFn: () => asignarEstudioTitulosService.getByIdExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}

