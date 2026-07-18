using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class realizar_revision_previo_firma_banco: base_auditoria
    {
        public int id_realizar_revision_previo_firma_banco { get; set; }
        public long id_expediente { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
