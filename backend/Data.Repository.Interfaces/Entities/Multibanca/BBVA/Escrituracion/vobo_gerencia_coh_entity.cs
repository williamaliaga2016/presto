using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion
{
    public class vobo_gerencia_coh_entity
    {
        [Key]
        public long      id             { get; set; }
        public long      id_expediente  { get; set; }
        public string    id_actividad   { get; set; } = string.Empty;

        // Campos de dictamen
        public string?   concepto_vobo  { get; set; }
        public string?   observaciones  { get; set; }

        // Campos de auditoría (explícitos, sin herencia)
        public bool      is_active      { get; set; } = true;
        public bool      row_status     { get; set; } = true;
        public int       created_by     { get; set; }
        public DateTime  created_date   { get; set; } = DateTime.Now;
        public int?      modified_by    { get; set; }
        public DateTime? modified_date  { get; set; }
    }
}
