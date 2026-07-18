using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Multibanca
{
    public class ConsultActivityDTO
    {
        public long id_expediente { get; set; }
        public string descripcion { get; set; }
        public string status { get; set; }
        public string descripcion_rol { get; set; }
        public string usuario { get; set; }
        public DateTime fecha_asignacion { get; set; }
    }
}
