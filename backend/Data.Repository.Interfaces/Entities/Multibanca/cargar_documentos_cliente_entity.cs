using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    /// <summary>
    /// Entidad de repositorio para la confirmacion documental de Cargar Documentos Cliente.
    /// </summary>
    public class cargar_documentos_cliente_entity
    {
        [Key]
        public long id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "BBVA_CONTACTO_CARGAR_DOCUMENTOS_CLIENTE_CBF7A738";
        public bool documentos_adjuntos { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
