import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GetByExpedienteResponse } from '../models/realizar_vb_final_abogado';
import { realizarVBFinalAbogadoService } from '../api/realizarVBFinalAbogadoService';

export function useRealizarVBFinalAbogado(id_expediente: number) {
  return useQuery<ApiResponse<GetByExpedienteResponse | null>>({
    queryKey: ['realizar_vb_final_abogado', id_expediente],
    queryFn: () => realizarVBFinalAbogadoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
