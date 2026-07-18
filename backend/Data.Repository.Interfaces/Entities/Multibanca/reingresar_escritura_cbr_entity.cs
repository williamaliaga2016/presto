using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class reingresar_escritura_cbr_entity
    {
        [Key]
        public int id_reingresar_escritura_cbr { get; set; }

        public long id_expediente { get; set; }

        public int id_usuario_solicitante { get; set; }

        public bool is_subsanar { get; set; }

        public string? observaciones { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
