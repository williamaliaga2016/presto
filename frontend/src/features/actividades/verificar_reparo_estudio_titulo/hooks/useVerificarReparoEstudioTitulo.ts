import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { VerificarReparoEstudioTitulo } from '../models/verificar_reparo_estudio_titulo';
import { verificarReparoEstudioTituloService } from '../api/verificarReparoEstudioTituloService';

export function useVerificarReparoEstudioTitulo(id_expediente: number) {
  return useQuery<ApiResponse<VerificarReparoEstudioTitulo | null>>({
    queryKey: ['verificar_reparo_estudio_titulo', id_expediente],
    queryFn: () => verificarReparoEstudioTituloService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
