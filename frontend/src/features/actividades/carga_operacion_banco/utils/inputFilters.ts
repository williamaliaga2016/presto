const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export const isValidEmail = (value?: string | null): boolean => {
  if (!value?.trim()) return true;
  return EMAIL_REGEX.test(value.trim());
};

export const onlyDigits = (value: string, maxLength = 10): string =>
  value.replace(/\D/g, '').slice(0, maxLength);

export const onlyLetters = (value: string): string =>
  value.replace(/[^a-zA-ZÀ-ÿñÑ\s]/g, '');

export const isPasaporte = (
  tipoDocumentoId: string | null | undefined,
  options: { code?: string | null; description?: string | null }[],
): boolean => {
  if (!tipoDocumentoId) return false;
  const option = options.find((item) => item.code === tipoDocumentoId);
  return (option?.description ?? '').toLowerCase().includes('pasaporte');
};

export const filterNumeroIdentificacion = (value: string, allowLetters: boolean): string =>
  allowLetters ? value.replace(/[^a-zA-Z0-9]/g, '') : value.replace(/\D/g, '');

export const formatPlazoLegible = (plazoMeses?: number | null): string => {
  if (!plazoMeses || plazoMeses <= 0) return '';

  const anios = Math.floor(plazoMeses / 12);
  const meses = plazoMeses % 12;
  const partes: string[] = [];

  if (anios > 0) partes.push(`${anios} ${anios === 1 ? 'año' : 'años'}`);
  if (meses > 0 || anios === 0) partes.push(`${meses} ${meses === 1 ? 'mes' : 'meses'}`);

  return partes.join(' y ');
};
