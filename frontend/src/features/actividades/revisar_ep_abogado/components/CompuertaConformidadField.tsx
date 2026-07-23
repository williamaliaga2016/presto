import DropdownForm from '@/shared/components/DropdownForm';
import type { CatalogoOption } from '@/models/CatalogoOption';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';

const CONFORMIDAD_OPTIONS: CatalogoOption[] = [
  { code: 'SI', description: 'SÍ' },
  { code: 'NO', description: 'NO' },
];

interface Props {
  form: RevisarEpAbogado;
  updateField: <K extends keyof RevisarEpAbogado>(
    field: K,
    value: RevisarEpAbogado[K],
  ) => void;
  onConformidadChange?: (value: RevisarEpAbogado['ep_conforme']) => void;
  disabled?: boolean;
}

/**
 * Campo obligatorio "¿Escritura Pública Conforme?" (SI/NO).
 * Emite onChange a través de updateField para que la página padre
 * controle la visibilidad de los campos condicionales (NovedadesSection).
 * Corresponde al requerimiento CA04.
 */
export default function CompuertaConformidadField({
  form,
  updateField,
  onConformidadChange,
  disabled = false,
}: Props) {
  const handleChange = (val: string | null) => {
    const newValue = val as RevisarEpAbogado['ep_conforme'];
    updateField('ep_conforme', newValue);

    // Cuando cambia a "SI", limpiar campos condicionales de novedades
    if (newValue === 'SI') {
      updateField('tipologia', null);
      updateField('casuistica', null);
      updateField('observaciones_legales', null);
    }

    onConformidadChange?.(newValue);
  };

  return (
    <DropdownForm
      label="¿Escritura Pública Conforme?"
      value={form.ep_conforme}
      options={CONFORMIDAD_OPTIONS}
      onChange={handleChange}
      placeholder="Seleccionar..."
      disabled={disabled}
      required
    />
  );
}
