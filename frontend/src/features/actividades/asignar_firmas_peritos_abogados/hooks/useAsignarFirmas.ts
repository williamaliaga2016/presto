import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { asignarFirmasService } from '../api/asignarFirmasService';
import type { AsignarFirmasForm, CalcularAsignacionRequest } from '../models/asignarFirmas';

export function useAsignarFirmas(id: number) {
  return useQuery({
    queryKey: ['asignar-firmas', id],
    queryFn: () => asignarFirmasService.getConEncabezado(id),
    enabled: id > 0,
  });
}

export function useControlesAsignarFirmas(id: number) {
  return useQuery({
    queryKey: ['asignar-firmas-controles', id],
    queryFn: () => asignarFirmasService.getControles(id),
    enabled: id > 0,
  });
}

export function useGuardarAsignarFirmas() {
  const client = useQueryClient();
  return useMutation({
    mutationFn: (payload: AsignarFirmasForm) => asignarFirmasService.guardar(payload),
    onSuccess: (_, payload) => client.invalidateQueries({
      queryKey: ['asignar-firmas', payload.id_expediente],
    }),
  });
}

export function useCalcularAsignacion(id: number) {
  return useMutation({
    mutationFn: (payload: CalcularAsignacionRequest) =>
      asignarFirmasService.calcular(id, payload),
  });
}

export function useAvanzarAsignarFirmas() {
  return useMutation({ mutationFn: (id: number) => asignarFirmasService.avanzar(id) });
}

