using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion
{
    public class revisar_ep_abogado_entity
    {
        [Key]
        public long      id             { get; set; }
        public long      id_expediente  { get; set; }
        public string    id_actividad   { get; set; } = "BBVA_ESCRITURACION_REVISAR_EP_ABOGADO";

        // Campo editable (herencia editable desde firmar_escritura_cliente)
        public string?   representante_legal { get; set; }

        // Compuerta de Conformidad
        public string?   ep_conforme         { get; set; } // "SI" | "NO" | null

        // Campos condicionales (obligatorios cuando ep_conforme = "NO")
        public string?   tipologia           { get; set; } // Parametría L39
        public string?   casuistica          { get; set; } // Parametría L40
        public string?   observaciones_legales { get; set; }

        // Auditoría
        public bool      is_active      { get; set; } = true;
        public bool      row_status     { get; set; } = true;
        public int       created_by     { get; set; }
        public DateTime  created_date   { get; set; } = DateTime.Now;
        public int?      modified_by    { get; set; }
        public DateTime? modified_date  { get; set; }
    }
}
