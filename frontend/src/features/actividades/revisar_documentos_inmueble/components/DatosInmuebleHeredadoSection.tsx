import ReadOnlyField from './ReadOnlyField';
import type { DatosInmuebleHeredado } from '../models/revisar_documentos_inmueble';

type Props = {
  data: DatosInmuebleHeredado;
};

export default function DatosInmuebleHeredadoSection({ data }: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
      <ReadOnlyField label="Tipo de Inmueble" value={data.tipo_inmueble} />
      <ReadOnlyField label="Estado del Inmueble" value={data.estado_inmueble} />
      <ReadOnlyField label="Constructora" value={data.constructora} />
      <ReadOnlyField label="Descripción del Proyecto" value={data.descripcion_proyecto} />
      <ReadOnlyField label="Departamento" value={data.departamento_inmueble} />
      <ReadOnlyField label="Municipio" value={data.municipio_inmueble} />
    </div>
  );
}