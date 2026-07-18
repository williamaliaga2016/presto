using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class recepcionar_matriz: base_auditoria
    {
        public int id_recepcionar_matriz { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
