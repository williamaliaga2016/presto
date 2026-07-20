import type { EncabezadoDTO } from "../models/encabezado";
import { useEncabezado } from "../hooks/useEncabezado";
import { Card } from "primereact/card";

type EncabezadoActividadProps = {
  idExpediente: number;
  activityID?: string;
};

const formatFecha = (value?: string | null) => {
  if (!value) return "-";

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;

  return date.toLocaleDateString("es-CO");
};

const formatImporte = (value?: number | null) => {
  if (value === null || value === undefined) return "-";

  return new Intl.NumberFormat("es-CO", {
    style: "currency",
    currency: "COP",
    minimumFractionDigits: 0,
  }).format(value);
};

const formatTasa = (value?: string | number | null) => {
  if (
    value === null ||
    value === undefined ||
    String(value).trim() === ""
  ) {
    return "-";
  }

  const numericValue =
    typeof value === "number"
      ? value
      : Number.parseFloat(String(value).replace(",", "."));

  if (Number.isNaN(numericValue)) {
    return String(value);
  }

  return `${Number(numericValue.toFixed(2))}%`;
};

function LabelItem({
  label,
  value,
}: {
  label: string;
  value?: string | number | null;
}) {
  return (
    <div className="flex flex-col gap-1">
      <span className="text-xs font-semibold uppercase tracking-wide text-gray-500">
        {label}
      </span>
      <span className="min-h-[20px] text-sm font-medium text-gray-800">
        {value !== null && value !== undefined && String(value).trim() !== ""
          ? String(value)
          : "-"}
      </span>
    </div>
  );
}

function EncabezadoContent({ data }: { data: EncabezadoDTO }) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-5">
      <LabelItem label="Id Expediente" value={data.id_expediente} />

      <LabelItem label="Actividad" value={data.actividad} />
      <LabelItem label="Estado" value={data.estado} />
      <LabelItem label="Usuario Asignado" value={data.usuario_asignado} />

      <LabelItem
        label="Fecha Alta"
        value={formatFecha(data.fecha_alta)}
      />
      <LabelItem
        label="Fecha Asignación"
        value={formatFecha(data.fecha_asignacion)}
      />

      <div className="col-span-full border-t border-gray-200 mt-2 pt-2" />
      <LabelItem label="ID Scoring" value={data.id_scoring} />
      <LabelItem label="SubProducto" value={data.id_tipo_sub_producto} />
      <LabelItem label="Titular Principal" value={data.nombre_completo_t1} />
      <LabelItem
        label={data.tipo_documento_id_t1 ?? 'Documento T1'}
        value={data.numero_identificacion_t1}
      />
      <LabelItem label="Celular T1" value={data.celular_t1} />
      <LabelItem label="Titular 2" value={data.nombre_completo_t2} />
      <LabelItem label="Documento T2" value={data.numero_identificacion_t2} />
      <LabelItem
        label="Cliente Nómina"
        value={data.cliente_nomina === null || data.cliente_nomina === undefined
          ? null
          : data.cliente_nomina ? 'Sí' : 'No'}
      />
      <LabelItem label="Situación Laboral" value={data.situacion_laboral} />
      <LabelItem label="Correo Declarativo" value={data.correo_declarativo} />
      <LabelItem label="Teléfono Declarativo" value={data.telefono_declarativo} />
      <LabelItem label="Monto Otorgado" value={formatImporte(data.monto_otorgado)} />
      <LabelItem label="Plazo Otorgado" value={data.plazo_otorgado} />
      <LabelItem label="Tasa" value={formatTasa(data.tasa)} />
      <LabelItem label="Canal Originación" value={data.canal_originacion} />
      <LabelItem label="Código Proyecto" value={data.codigo_proyecto} />
      <LabelItem label="Proyecto" value={data.descripcion_proyecto} />
      <LabelItem label="Estado Inmueble" value={data.estado_inmueble} />
      <LabelItem label="Tipo Inmueble" value={data.tipo_inmueble} />
      <LabelItem label="Condiciones Organismo Decisor" value={data.condiciones_organismo_decisor} />
      <LabelItem
        label="Oficina BBVA"
        value={data.codigo_oficina_bbva
          ? `${data.codigo_oficina_bbva} - ${data.descripcion_oficina_bbva ?? ''}`
          : data.descripcion_oficina_bbva}
      />
      <LabelItem label="Código Asesor BBVA" value={data.codigo_asesor_bbva} />
      <LabelItem label="Fecha Aprobación" value={formatFecha(data.fecha_aprobacion)} />
    </div>
  );
}

export default function EncabezadoActividad({
  idExpediente,
  activityID,
}: EncabezadoActividadProps) {
  const { data, isLoading, isError } = useEncabezado(idExpediente, activityID);

  // Si es un registro nuevo y todavía no hay expediente, no mostrar nada
  if (!idExpediente || idExpediente <= 0) {
    return null;
  }

  if (isLoading) {
    return (
      <div className="w-full rounded-xl border border-blue-100 bg-blue-50 px-4 py-4 mb-6">
        <span className="text-sm text-blue-700">
          Cargando información del encabezado...
        </span>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="w-full rounded-xl border border-red-200 bg-red-50 px-4 py-4 mb-6">
        <span className="text-sm text-red-700">
          Ocurrió un error al cargar la información del encabezado.
        </span>
      </div>
    );
  }

  if (!data?.status || !data.detail) {
    return (
      <div className="w-full rounded-xl border border-yellow-200 bg-yellow-50 px-4 py-4 mb-6">
        <span className="text-sm text-yellow-800">
          No se encontró información de encabezado para este expediente.
        </span>
      </div>
    );
  }

  return (
      <Card className="w-full shadow-md card-presto-form mb-6">

        <div className="mb-4">
          <p className="text-sm text-gray-500">
            Datos informativos de la actividad actual.
          </p>
        </div>

        <EncabezadoContent data={data.detail} />

      </Card>
  );
}
