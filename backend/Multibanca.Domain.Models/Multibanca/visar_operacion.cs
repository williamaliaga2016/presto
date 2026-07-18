using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class visar_operacion : base_auditoria
    {
        public int id_visar_operacion { get; set; }
        public long id_expediente { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
