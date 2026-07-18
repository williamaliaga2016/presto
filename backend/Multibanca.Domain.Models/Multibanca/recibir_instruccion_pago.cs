using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class recibir_instruccion_pago : base_auditoria
    {
        public int id_recibir_instruccion_pago { get; set; }
        public long id_expediente { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? condicion_especial_desembolso { get; set; }
        public string? observaciones { get; set; }
    }
}
