using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca.BBVA
{
    public class devolucion_vb_comercial : base_auditoria
    {
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_DEVOLUCION_VB_COMERCIAL";
        public bool? cliente_desiste { get; set; }
        public string? motivo_cierre { get; set; }
        public string? observaciones { get; set; }
    }
}
