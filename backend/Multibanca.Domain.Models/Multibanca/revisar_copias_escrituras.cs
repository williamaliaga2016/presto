using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class revisar_copias_escrituras: base_auditoria
    {
        public int id_revisar_copias_escrituras { get; set; }
        public long id_expediente { get; set; }
        public string? cbr { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
