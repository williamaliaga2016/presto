import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RegistrarFechaRegistroCbr } from '../models/registrar_fecha_registro_cbr';
import { registrarFechaRegistroCbrService } from '../api/registrarFechaRegistroCbrService';

export function useRegistrarFechaRegistroCbr(id_expediente: number) {
  return useQuery<ApiResponse<RegistrarFechaRegistroCbr | null>>({
    queryKey: ['registrar_fecha_registro_cbr', id_expediente],
    queryFn: () => registrarFechaRegistroCbrService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
