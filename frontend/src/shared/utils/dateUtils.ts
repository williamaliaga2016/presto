/**
 * Normaliza un valor de fecha a formato "YYYY-MM-DD" para enviar al backend.
 * Retorna null si el valor es vacío o no es una fecha válida.
 */
export const normalizeDate = (value?: string | null): string | null => {
  if (!value) return null;

  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return null;
  }

  const yyyy = parsed.getFullYear();
  const mm = String(parsed.getMonth() + 1).padStart(2, '0');
  const dd = String(parsed.getDate()).padStart(2, '0');

  return `${yyyy}-${mm}-${dd}`;
};

/**
 * Convierte un string de fecha a objeto Date para componentes Calendar.
 * Retorna null si el valor es vacío o no es una fecha válida.
 * Usa parsing manual para evitar problemas de timezone con fechas YYYY-MM-DD.
 */
export const toDateValue = (value?: string | null): Date | null => {
  if (!value) return null;

  // Si es formato YYYY-MM-DD, parsear manualmente para evitar timezone offset
  const match = value.match(/^(\d{4})-(\d{2})-(\d{2})/);
  if (match) {
    return new Date(Number(match[1]), Number(match[2]) - 1, Number(match[3]));
  }

  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? null : parsed;
};

/**
 * Formatea una fecha ISO (ej: "2026-07-23T00:00:00") a formato dd/mm/yyyy.
 * Retorna '—' si el valor es nulo o inválido.
 */
export const formatDate = (value?: string | null): string => {
  if (!value) return '—';

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;

  return date.toLocaleDateString('es-CO', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  });
};
