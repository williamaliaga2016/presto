using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class revisar_documentos_inmueble_entity
    {
        [Key]
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "BBVA_CONTACTO_REVISAR_DOCUMENTOS_INMUEBLE_810753B8";
        public bool? documentos_correctos { get; set; }
        public string? motivo_devolucion { get; set; }
        public string? observaciones { get; set; }
        public string? requiere_actualizacion_avaluos { get; set; }
        public string? homologacion { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
