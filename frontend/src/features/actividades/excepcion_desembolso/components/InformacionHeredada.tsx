/**
 * InformacionHeredada
 *
 * Muestra en modo solo lectura los campos heredados de la actividad previa,
 * adaptando el contenido según el origen del caso:
 *
 * - FIRMAR_REP_LEGAL  : ConceptoFirma
 * - RECEPCION_BOLETA  : ConceptoFirma + FechaBoleta, NumeroBoleta, TipoBoleta, OficinaRegistro
 *
 * Req: 2.1, 2.2, 2.3, 2.6
 */
import type { ExcepcionDesembolsoHerencia } from '../models/excepcion_desembolso';

// ─── Helper ──────────────────────────────────────────────────────────────────

/** Campo de solo lectura. Muestra la etiqueta siempre; si no hay valor muestra vacío (Req 2.6). */
function ReadonlyField({
  label,
  value,
}: {
  label: string;
  value: string | null | undefined;
}) {
  return (
    <div className="flex flex-col gap-0.5">
      <span className="text-xs font-semibold uppercase tracking-wide text-gray-500">
        {label}
      </span>
      <span className="text-sm font-medium text-gray-800">
        {value ?? ''}
      </span>
    </div>
  );
}

// ─── Props ───────────────────────────────────────────────────────────────────

interface InformacionHeredadaProps {
  herencia: ExcepcionDesembolsoHerencia | null;
  origenCaso: string | null;
}

// ─── Componente ──────────────────────────────────────────────────────────────

export default function InformacionHeredada({
  herencia,
  origenCaso,
}: InformacionHeredadaProps) {
  if (!herencia) return null;

  return (
    <div className="space-y-4">
      {/* Origen del caso */}
      {origenCaso && (
        <div className="flex items-center gap-2">
          <span className="text-xs font-semibold uppercase tracking-wide text-gray-500">
            Origen del caso:
          </span>
          <span className="text-xs font-medium text-gray-700">{origenCaso}</span>
        </div>
      )}

      {/* Campos comunes a ambos orígenes */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Información de Firma
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-3">
          <ReadonlyField label="Concepto de Firma" value={herencia.conceptoFirmaDesc} />
        </div>
      </div>

      {/* Campos exclusivos de RECEPCION_BOLETA */}
      {herencia.esRecepcionBoleta && (
        <div>
          <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
            Datos de Radicación en Oficina de Registro
          </h4>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-x-4 gap-y-3">
            <ReadonlyField label="Fecha de Ingreso"    value={herencia.fechaBoleta} />
            <ReadonlyField label="Radicado"            value={herencia.numeroBoleta} />
            <ReadonlyField label="Tipo de Boleta"      value={herencia.tipoBoleta} />
            <ReadonlyField label="Oficina de Registro" value={herencia.oficinaRegistro} />
          </div>
        </div>
      )}
    </div>
  );
}
