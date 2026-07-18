using Common.Domain.Models;
using System.Collections.Generic;

namespace Multibanca.Domain.Models.Multibanca
{
    public class tasacion : base_auditoria
    {
        public int id_tasacion { get; set; }
        public long id_expediente { get; set; }
        public bool is_enviar_reparo { get; set; }
        public string? observaciones { get; set; }

        public List<tasacion_detalle>? detalles { get; set; }
    }
}
