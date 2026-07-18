using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class rectificatoria_firma_detalle_entity
    {
        [Key]
        public int id_rectificatoria_firma_detalle { get; set; }
        public int id_rectificatoria_firma { get; set; }
        public long id_expediente { get; set; }
        public string rol_compadecencia { get; set; } = string.Empty;
        public string rut { get; set; } = string.Empty;
        public DateTime fecha_firma { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
