using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class verificar_reparo_estudio_titulo : base_auditoria
    {
        public int id_verificar_reparo_estudio_titulo { get; set; }
        public long id_expediente { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
