import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GetByExpedienteResponse } from '../models/realizar_gestion_comercial';
import { realizarGestionComercialService } from '../api/realizarGestionComercialService';

export function useRealizarGestionComercial(id_expediente: number) {
  return useQuery<ApiResponse<GetByExpedienteResponse | null>>({
    queryKey: ['realizar_gestion_comercial', id_expediente],
    queryFn: () => realizarGestionComercialService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
