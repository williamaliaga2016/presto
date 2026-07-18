using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class registrar_firma_apoderado_banco : base_auditoria
    {
        public int id_registrar_firma_apoderado_banco { get; set; }
        public DateTime fecha_firma { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
