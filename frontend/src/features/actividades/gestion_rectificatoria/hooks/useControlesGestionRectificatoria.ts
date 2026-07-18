import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { GestionRectificatoriaService } from '../api/gestionRectificatoriaService';
import type { ControlesGestionRectificatoria } from '../models/catalogo';

export function useControlesGestionRectificatoria(enabled = true) {
  return useQuery<ApiResponse<ControlesGestionRectificatoria>>({
    queryKey: ['catalogo'],
    queryFn: () => GestionRectificatoriaService.getControles(),
    enabled,
    staleTime: 1000 * 60 * 10,
  });
}
