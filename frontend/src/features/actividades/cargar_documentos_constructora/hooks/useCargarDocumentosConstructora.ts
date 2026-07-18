import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CargarDocumentosConstructora } from '../models/cargar_documentos_constructora';
import { cargarDocumentosConstructoraService } from '../api/cargarDocumentosConstructoraService';

export const cargarDocumentosConstructoraQueryKeys = {
  actividad: (id_expediente: number) =>
    ['cargar-documentos-constructora', id_expediente] as const,
};

export function useCargarDocumentosConstructora(id_expediente: number) {
  return useQuery<ApiResponse<CargarDocumentosConstructora | null>>({
    queryKey: cargarDocumentosConstructoraQueryKeys.actividad(id_expediente),
    queryFn: () =>
      cargarDocumentosConstructoraService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
