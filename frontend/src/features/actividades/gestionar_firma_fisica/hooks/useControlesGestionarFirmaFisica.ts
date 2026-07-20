import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { gestionarFirmaFisicaService } from '../api/gestionarFirmaFisicaService';
import type { ControlesGestionarFirmaFisica } from '../models/controles';

export function useControlesGestionarFirmaFisica(id_expediente: number) {
  return useQuery<ApiResponse<ControlesGestionarFirmaFisica>>({
    queryKey: ['gestionar_firma_fisica_controles', id_expediente],
    queryFn: () => gestionarFirmaFisicaService.getControles(),
    enabled: !!id_expediente,
  });
}
