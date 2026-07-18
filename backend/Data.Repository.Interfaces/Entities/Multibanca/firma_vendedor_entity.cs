using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class firma_vendedor_entity
    {
        [Key]
        public int id_firma_vendedor { get; set; }
        public long id_expediente { get; set; }
        public string observaciones { get; set; } = string.Empty;

        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
