import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { cargarDocumentosClienteService } from '../api/cargarDocumentosClienteService';
import type {
  CargarDocumentosCliente,
  CargarDocumentosClienteInfo,
} from '../models/cargar_documentos_cliente';

/**
 * Obtiene el registro de confirmacion documental de Cargar Documentos Cliente para un expediente.
 *
 * @param id_expediente Identificador del expediente en Presto.
 * @returns Query de React Query con el registro activo o `null`.
 */
export function useCargarDocumentosCliente(id_expediente: number) {
  return useQuery<ApiResponse<CargarDocumentosCliente | null>>({
    queryKey: ['cargar-documentos-cliente', id_expediente],
    queryFn: () => cargarDocumentosClienteService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}

/**
 * Obtiene la informacion general que se muestra en la pantalla externa de Cargar Documentos Cliente.
 *
 * @param id_expediente Identificador del expediente en Presto.
 * @returns Query de React Query con datos generales de cliente y analista.
 */
export function useCargarDocumentosClienteInfo(id_expediente: number) {
  return useQuery<ApiResponse<CargarDocumentosClienteInfo>>({
    queryKey: ['cargar-documentos-cliente-info', id_expediente],
    queryFn: () => cargarDocumentosClienteService.getInfoCliente(id_expediente),
    enabled: id_expediente > 0,
  });
}

/**
 * Guarda la confirmacion documental e invalida la consulta del registro de Cargar Documentos Cliente.
 *
 * @param id_expediente Identificador del expediente usado como llave de cache.
 * @returns Mutation de React Query para persistir la confirmacion documental.
 */
export function useGuardarCargarDocumentosCliente(id_expediente: number) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (payload: CargarDocumentosCliente) =>
      cargarDocumentosClienteService.guardar(payload),
    onSuccess: () => {
      void queryClient.invalidateQueries({
        queryKey: ['cargar-documentos-cliente', id_expediente],
      });
    },
  });
}

/**
 * Ejecuta el avance de Cargar Documentos Cliente cuando la confirmacion documental ya fue guardada.
 *
 * @returns Mutation de React Query para avanzar la actividad.
 */
export function useAvanzarCargarDocumentosCliente() {
  return useMutation({
    mutationFn: (id_expediente: number) =>
      cargarDocumentosClienteService.avanzar(id_expediente),
  });
}
