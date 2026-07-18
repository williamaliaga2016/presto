using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class verificar_reparo_cbr_entity
    {
        [Key]
        public int id_verificar_reparo_cbr { get; set; }
        public long id_expediente { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? enviar_reparo_a { get; set; }
        public bool? estatus_reparo { get; set; }
        public string? observaciones { get; set; }

        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}

