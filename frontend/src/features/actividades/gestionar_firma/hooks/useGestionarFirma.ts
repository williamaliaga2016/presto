import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionarFirma } from '../models/gestionar_firma';
import { gestionarFirmaService } from '../api/gestionarFirmaService';

export function useGestionarFirma(id_expediente: number) {
  return useQuery<ApiResponse<GestionarFirma | null>>({
    queryKey: ['gestionar_firma', id_expediente],
    queryFn: () => gestionarFirmaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
