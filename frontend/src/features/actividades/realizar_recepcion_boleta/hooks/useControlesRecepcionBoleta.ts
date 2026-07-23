import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesRecepcionBoleta } from '../models/controles';
import { realizarRecepcionBoletaService } from '../api/realizarRecepcionBoletaService';

export function useControlesRecepcionBoleta() {
  return useQuery<ApiResponse<ControlesRecepcionBoleta>>({
    queryKey: ['controles_recepcion_boleta'],
    queryFn: () => realizarRecepcionBoletaService.getControles(),
  });
}
