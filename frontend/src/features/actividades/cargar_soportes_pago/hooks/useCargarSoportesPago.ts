import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { cargarSoportesPagoService } from '../api/cargarSoportesPagoService';
import type {
  CargarSoportesPago,
  CargarSoportesPagoInfo,
} from '../models/cargar_soportes_pago';

/**
 * Obtiene el registro de confirmacion documental de Cargar Soportes de Pago para un expediente.
 *
 * @param id_expediente Identificador del expediente en Presto.
 * @returns Query de React Query con el registro activo o `null`.
 */
export function useCargarSoportesPago(id_expediente: number) {
  return useQuery<ApiResponse<CargarSoportesPago | null>>({
    queryKey: ['cargar-soportes-pago', id_expediente],
    queryFn: () => cargarSoportesPagoService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}

/**
 * Obtiene la informacion general que se muestra en la pantalla externa de Cargar Soportes de Pago.
 *
 * @param id_expediente Identificador del expediente en Presto.
 * @returns Query de React Query con datos generales de cliente y analista.
 */
export function useCargarSoportesPagoInfo(id_expediente: number) {
  return useQuery<ApiResponse<CargarSoportesPagoInfo>>({
    queryKey: ['cargar-soportes-pago-info', id_expediente],
    queryFn: () => cargarSoportesPagoService.getInfoCliente(id_expediente),
    enabled: id_expediente > 0,
  });
}

/**
 * Guarda la confirmacion documental e invalida la consulta del registro de Cargar Soportes de Pago.
 *
 * @param id_expediente Identificador del expediente usado como llave de cache.
 * @returns Mutation de React Query para persistir la confirmacion documental.
 */
export function useGuardarCargarSoportesPago(id_expediente: number) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (payload: CargarSoportesPago) =>
      cargarSoportesPagoService.guardar(payload),
    onSuccess: () => {
      void queryClient.invalidateQueries({
        queryKey: ['cargar-soportes-pago', id_expediente],
      });
    },
  });
}

/**
 * Ejecuta el avance de Cargar Soportes de Pago cuando la confirmacion documental ya fue guardada.
 *
 * @returns Mutation de React Query para avanzar la actividad.
 */
export function useAvanzarCargarSoportesPago() {
  return useMutation({
    mutationFn: (id_expediente: number) =>
      cargarSoportesPagoService.avanzar(id_expediente),
  });
}
