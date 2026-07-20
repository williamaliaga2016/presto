import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GestionarFirmaFisica } from '../models/gestionar_firma_fisica';
import { gestionarFirmaFisicaService } from '../api/gestionarFirmaFisicaService';

export function useGestionarFirmaFisica(id_expediente: number) {
  return useQuery<ApiResponse<GestionarFirmaFisica | null>>({
    queryKey: ['gestionar_firma_fisica', id_expediente],
    queryFn: () =>
      gestionarFirmaFisicaService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
