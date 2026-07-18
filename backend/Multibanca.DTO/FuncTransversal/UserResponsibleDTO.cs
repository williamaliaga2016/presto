using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.FuncTransversal
{
    public class UserResponsibleDTO
    {
        public long id_expediente { get; set; }
        public string descripcion { get; set; }
        public string status { get; set; }
        public string rol { get; set; }
        public int id_usuario { get; set; }
        public string user_name { get; set; }
        public string nombre_responsable { get; set; }
        public DateTime fecha_asignacion { get; set; }
    }
}
