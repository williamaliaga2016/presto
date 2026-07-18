import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarInscripcionCbr } from '../models/revisar_inscripcion_cbr';
import { revisarInscripcionCbrService } from '../api/revisarInscripcionCbrService';

export function useRevisarInscripcionCbr(id_expediente: number) {
  return useQuery<ApiResponse<RevisarInscripcionCbr | null>>({
    queryKey: ['revisar_inscripcion_cbr', id_expediente],
    queryFn: () => revisarInscripcionCbrService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
