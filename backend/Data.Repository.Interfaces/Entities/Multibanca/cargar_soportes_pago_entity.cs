using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    /// <summary>
    /// Entidad de repositorio para la confirmacion de soportes de pago.
    /// </summary>
    public class cargar_soportes_pago_entity
    {
        [Key]
        public long id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "BBVA_CONTACTO_CARGAR_SOPORTES_DE_PAGO_899F408B";
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
