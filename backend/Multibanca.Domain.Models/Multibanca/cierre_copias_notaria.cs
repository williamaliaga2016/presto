using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class cierre_copias_notaria : base_auditoria
    {
        public int id_cierre_copias_notaria { get; set; }
        public long id_expediente { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
