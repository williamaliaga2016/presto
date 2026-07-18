using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class reparo_cdr_detalle_entity
    {
        [Key]
        public int id_reparo_cdr { get; set; }
        public long id_expediente { get; set; }
        public string? solicitante { get; set; }
        public string? observaciones { get; set; }
        public DateTime? fecha_ingreso { get; set; }
        public bool subsanar { get; set; }

        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
