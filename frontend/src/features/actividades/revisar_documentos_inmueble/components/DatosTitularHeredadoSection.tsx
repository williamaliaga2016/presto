import ReadOnlyField from './ReadOnlyField';
import type { DatosTitularHeredado } from '../models/revisar_documentos_inmueble';

type Props = {
  data: DatosTitularHeredado;
};

export default function DatosTitularHeredadoSection({ data }: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
      <ReadOnlyField label="Tipo de Identificación" value={data.tipo_identificacion} />
      <ReadOnlyField label="Número de Identificación" value={data.numero_identificacion} />
      <ReadOnlyField label="Nombre Completo (Titular 1)" value={data.nombre_completo_t1} />
      <ReadOnlyField label="Situación Laboral" value={data.situacion_laboral} />
    </div>
  );
}