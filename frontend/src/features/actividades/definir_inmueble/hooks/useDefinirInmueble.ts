import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from
  '../../validar_informacion/models/catalogo';
import type { ControlesValidarInformacion } from
  '../../validar_informacion/models/catalogo';
import type {
  DefinirInmuebleAvanzarResponse,
  DefinirInmuebleBBVA,
} from '../models/definir_inmueble';
import { definirInmuebleService } from '../api/definirInmuebleService';

export function useDefinirInmueble(id_expediente: number) {
  return useQuery<ApiResponse<DefinirInmuebleBBVA | null>>({
    queryKey: ['definir-inmueble', id_expediente],
    queryFn: () => definirInmuebleService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}

export function useControlesDefinirInmueble(id_expediente: number) {
  return useQuery<ApiResponse<ControlesValidarInformacion>>({
    queryKey: ['controles-definir-inmueble', id_expediente],
    queryFn: () => definirInmuebleService.getControles(),
    enabled: id_expediente > 0,
    placeholderData: {
      status: true,
      detail: EMPTY_CONTROLES_VALIDAR_INFORMACION,
      message: '',
    },
  });
}

export function useUpsertDefinirInmueble() {
  const queryClient = useQueryClient();
  return useMutation<
    ApiResponse<DefinirInmuebleBBVA>,
    Error,
    DefinirInmuebleBBVA
  >({
    mutationFn: (payload) => definirInmuebleService.guardar(payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['definir-inmueble', variables.id_expediente],
      });
    },
  });
}

export function useAvanzarDefinirInmueble() {
  return useMutation<
    ApiResponse<DefinirInmuebleAvanzarResponse>,
    Error,
    { id_expediente: number; confirmar: boolean }
  >({
    mutationFn: ({ id_expediente, confirmar }) =>
      definirInmuebleService.avanzar(id_expediente, confirmar),
  });
}
