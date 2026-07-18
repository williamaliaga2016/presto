using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class rectificatoria_analisis_derivacion_reparo_postventa : base_auditoria
    {
        public int id_rectificatoria_analisis_derivacion_reparo_postventa { get; set; }
        public long id_expediente { get; set; }
        public string? enviar_reparo_a { get; set; }
        public string? observaciones { get; set; }
    }
}
