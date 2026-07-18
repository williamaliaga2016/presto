using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Multibanca
{
    public class EtapaDTO
    {
        public int id_etapa { get; set; }
        public string etapa { get; set; }
        public int current_actividades { get; set; }
        public List<ActividadDTO> actividades { get; set; }

        // Extras
        public string estado { get; set; }
        public string[] title { get; set; }
    }
}
