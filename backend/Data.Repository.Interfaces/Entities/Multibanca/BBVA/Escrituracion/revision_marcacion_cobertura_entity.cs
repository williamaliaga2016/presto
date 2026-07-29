using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion
{
    public class revision_marcacion_cobertura_entity
    {
        [Key]
        public long      id             { get; set; }
        public long      id_expediente  { get; set; }
        public string    id_actividad   { get; set; } = string.Empty;

        // CA05 — Notificación a Colocaciones (bloqueante, CA08)
        public string?   email_area_colocaciones { get; set; }

        // Identificación
        public string?   consecutivo             { get; set; }
        public string?   tipo_documento           { get; set; }
        public string?   numero_documento         { get; set; }
        public string?   tipo_tramite             { get; set; }
        public string?   nombre                   { get; set; }

        // Proyecto / Constructora (Diligencia SITCAR)
        public string?   constructora                 { get; set; }
        public string?   proyecto                     { get; set; }
        public DateTime? fecha_aceptacion_plataforma   { get; set; }
        public string?   tipo_vivienda                { get; set; }

        // Valores y obligación
        public decimal?  valor_subsidio           { get; set; }
        public string?   numero_obligacion        { get; set; }
        public DateTime? fecha_desembolso         { get; set; }
        public DateTime? fecha_proximo_canon      { get; set; }
        public decimal?  valor_desembolso         { get; set; }
        public decimal?  intereses_corrientes     { get; set; }
        public decimal?  capital                  { get; set; }
        public decimal?  seguros                  { get; set; }
        public decimal?  cuota_mensual            { get; set; }
        public int?      plazo                    { get; set; }
        public string?   observacion              { get; set; }

        // Solicitud / Respuesta de marcación (SITCAR)
        public DateTime? fecha_solicitud_marcacion { get; set; }
        public string?   hora_solicitud_marcacion  { get; set; }
        public DateTime? fecha_respuesta_marcacion { get; set; }
        public string?   hora_respuesta_marcacion  { get; set; }
        public string?   responsable_m5            { get; set; }

        // Resolución
        public string?   no_resolucion            { get; set; }
        public DateTime? fecha_resolucion         { get; set; }
        public DateTime? fecha_envio_resolucion   { get; set; }
        public string?   estado_proceso           { get; set; }

        // CA07 — Observaciones generales
        public string?   observaciones            { get; set; }

        // Campos de auditoría (explícitos, sin herencia)
        public bool      is_active      { get; set; } = true;
        public bool      row_status     { get; set; } = true;
        public int       created_by     { get; set; }
        public DateTime  created_date   { get; set; } = DateTime.Now;
        public int?      modified_by    { get; set; }
        public DateTime? modified_date  { get; set; }
    }
}
