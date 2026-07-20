import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CargarDocumentosConstructora } from '../models/cargar_documentos_constructora';
import { cargarDocumentosConstructoraService } from '../api/cargarDocumentosConstructoraService';

export function useCargarDocumentosConstructora(id_expediente: number) {
  return useQuery<ApiResponse<CargarDocumentosConstructora | null>>({
    queryKey: ['cargar_documentos_constructora', id_expediente],
    queryFn: () => cargarDocumentosConstructoraService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
