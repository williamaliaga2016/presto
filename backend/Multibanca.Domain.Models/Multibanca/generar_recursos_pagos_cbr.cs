using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class generar_recursos_pagos_cbr : base_auditoria
    {
        public int id_generar_recursos_pagos_cbr { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
