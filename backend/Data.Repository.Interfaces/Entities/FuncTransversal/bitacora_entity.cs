using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.FuncTransversal
{
    public class bitacora_entity
    {
        [Key]
        public long id_bitacora { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; }
        public int id_usuario { get; set; }
        public DateTime fecha_alta { get; set; }
        public string observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
