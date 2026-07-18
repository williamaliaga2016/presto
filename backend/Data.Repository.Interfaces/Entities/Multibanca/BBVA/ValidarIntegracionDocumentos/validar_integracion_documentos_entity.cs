using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos
{
    public class validar_integracion_documentos_entity
    {
        [Key]
        public int id { get; set; }

        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_VALIDAR_INTEGRACION";
        public bool? documentos_correctos { get; set; }
        public bool credito_condicionado { get; set; } = false;
        public string? motivo_devolucion { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; } = true;
        public bool row_status { get; set; } = true;
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}