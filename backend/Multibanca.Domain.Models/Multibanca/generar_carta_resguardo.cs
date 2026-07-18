using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class generar_carta_resguardo : base_auditoria
    {
        public int id_generar_carta_resguardo { get; set; }
        public long id_expediente { get; set; }
        public bool? generar_carta { get; set; }
        public string? tipo_carta { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
