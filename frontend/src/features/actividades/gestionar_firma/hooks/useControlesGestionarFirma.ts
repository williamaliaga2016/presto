import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { gestionarFirmaService } from '../api/gestionarFirmaService';
import type { ControlesGestionarFirma } from '../models/controles';

export function useControlesGestionarFirma(id_expediente: number) {
  return useQuery<ApiResponse<ControlesGestionarFirma>>({
    queryKey: ['gestionar_firma_controles', id_expediente],
    queryFn: () => gestionarFirmaService.getControles(),
    enabled: !!id_expediente,
  });
}
