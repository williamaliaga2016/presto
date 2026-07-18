using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class registrar_escritura_cbr_entity
    {
        [Key]
        public int id_registrar_escritura_cbr { get; set; }
        public long id_expediente { get; set; }
        public string? conservador { get; set; }
        public string? numero_caratula { get; set; }
        public string? observaciones { get; set; }

        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
