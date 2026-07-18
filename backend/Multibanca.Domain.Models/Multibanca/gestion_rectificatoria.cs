using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class gestion_rectificatoria : base_auditoria
    {
        public int id_gestion_rectificatoria { get; set; }
        public long id_expediente { get; set; }
        public string enviar_tipo_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
