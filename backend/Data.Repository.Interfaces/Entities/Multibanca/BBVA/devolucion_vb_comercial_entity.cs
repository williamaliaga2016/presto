using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA
{
    public class devolucion_vb_comercial_entity
    {
        [Key]
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_DEVOLUCION_VB_COMERCIAL";
        public bool? cliente_desiste { get; set; }
        public string? motivo_cierre { get; set; }
        public string? observaciones { get; set; }

        // Propiedades explícitas si no se heredan
        public bool is_active { get; set; } = true;
        public bool row_status { get; set; } = true;
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
