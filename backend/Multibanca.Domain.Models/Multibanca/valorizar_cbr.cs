using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class valorizar_cbr : base_auditoria
    {
        public int id_valorizar_cbr { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
