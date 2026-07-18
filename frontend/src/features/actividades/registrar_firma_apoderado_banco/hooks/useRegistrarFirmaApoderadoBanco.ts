import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarFirmaApoderadoBanco } from '../models/registrar_firma_apoderado_banco';
import { registrarFirmaApoderadoBancoService } from '../api/registrarFirmaApoderadoBancoService';

export function useRegistrarFirmaApoderadoBanco(id_expediente: number) {
  return useQuery<ApiResponse<RegistrarFirmaApoderadoBanco | null>>({
    queryKey: ['registrar_firma_apoderado_banco', id_expediente],
    queryFn: () => registrarFirmaApoderadoBancoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}