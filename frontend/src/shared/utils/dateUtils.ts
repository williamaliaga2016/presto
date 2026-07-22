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
 */
export const toDateValue = (value?: string | null): Date | null => {
  if (!value) return null;

  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? null : parsed;
};
