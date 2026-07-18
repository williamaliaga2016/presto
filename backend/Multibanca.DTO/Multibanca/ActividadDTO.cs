using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Multibanca
{
    public class ActividadDTO
    {
        public long nro_row { get; set; }

        public long id_expediente { get; set; }

        public string? id_actividad { get; set; }

        public string? actividad { get; set; }

        public DateTime? fecha_asignacion { get; set; }

        public string? estado { get; set; }

        public string? rol { get; set; }

        public string? nombre_responsable { get; set; }

        public string? url_act { get; set; }

        // Extras
        public int id_etapa { get; set; }

        public string[]? title { get; set; }
    }
}
