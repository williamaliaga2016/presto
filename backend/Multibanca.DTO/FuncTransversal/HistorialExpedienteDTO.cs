using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.FuncTransversal
{
    public class HistorialExpedienteDTO
    {
        public string actividad { get; set; }

        public string status { get; set; }

        public string usuario { get; set; }

        public string rol { get; set; }

        public DateTime? fecha_inicio { get; set; }

        public DateTime? fecha_termino { get; set; }

        public long orden { get; set; }
    }
}
