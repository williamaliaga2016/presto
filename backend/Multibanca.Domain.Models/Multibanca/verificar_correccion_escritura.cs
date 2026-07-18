using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class verificar_correccion_escritura : base_auditoria
    {
        public int id_verificar_correccion_escritura { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
