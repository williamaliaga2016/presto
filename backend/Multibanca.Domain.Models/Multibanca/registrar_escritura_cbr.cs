using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class registrar_escritura_cbr : base_auditoria
    {
        public int id_registrar_escritura_cbr { get; set; }
        public long id_expediente { get; set; }
        public string? conservador { get; set; }
        public string? numero_caratula { get; set; }
        public string? observaciones { get; set; }
    }
}
