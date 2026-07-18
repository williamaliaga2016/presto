using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class registrar_fecha_registro_cbr : base_auditoria
    {
        public int id_registrar_fecha_registro_cbr { get; set; }
        public long id_expediente { get; set; }
        public DateTime? fecha_registro_cbr { get; set; }
        public string? observaciones { get; set; }
    }
}
