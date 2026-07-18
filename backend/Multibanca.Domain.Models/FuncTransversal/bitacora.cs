using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.FuncTransversal
{
    public class bitacora : base_auditoria
    {
        public long id_bitacora { get; set; }

        public long id_expediente { get; set; }
        public int id_usuario { get; set; }
        public string id_actividad { get; set; }
        public DateTime fecha_alta { get; set; }
        public string observaciones { get; set; }

        // Extras
        public string? actividad { get; set; }
        public string? usuario { get; set; }
        public string? rol { get; set; }
    }
}
