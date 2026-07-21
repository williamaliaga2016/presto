using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA
{
    public class firmar_escritura_cliente_entity
    {
        [Key]
        public long      id             { get; set; }
        public long      id_expediente  { get; set; }
        public string    id_actividad   { get; set; } = "BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F";

        // Bloque Información de Notaría (heredado, editable)
        // Heredado de Actividad (Validar Cumplimiento de politicas)
        public string?   notaria        { get; set; }
        public DateTime? fecha_notaria  { get; set; }
        public int?      numero_notaria { get; set; }
        public string?   ciudad_notaria { get; set; }

        // Bloque Formalización de Escritura
        public string?   numero_escritura    { get; set; }
        public DateTime? fecha_escritura     { get; set; }
        public string?   representante_legal { get; set; }

        // Decisiones de Enrutamiento
        public string?   requiere_escalamiento_comercial { get; set; } // "SI" | "NO" | null
        public string?   tipologia                       { get; set; }
        public string?   requiere_causar                 { get; set; } // "SI" | "NO" | null (solo Leasing)

        // Campos adicionales
        public string?   observaciones  { get; set; }
        public string?   tipo_credito   { get; set; }

        // Auditoría
        public bool      is_active      { get; set; } = true;
        public bool      row_status     { get; set; } = true;
        public int       created_by     { get; set; }
        public DateTime  created_date   { get; set; } = DateTime.Now;
        public int?      modified_by    { get; set; }
        public DateTime? modified_date  { get; set; }
    }
}
