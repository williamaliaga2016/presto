using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca.BBVA.ValidarIntegracionDocumentos
{
    public class interviniente_bbva : base_auditoria
    {
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_VALIDAR_INTEGRACION";
        public string nombre_completo { get; set; } = string.Empty;
        public string tipo_identificacion { get; set; } = string.Empty;
        public string numero_identificacion { get; set; } = string.Empty;
        public string? telefono { get; set; }
        public string? correo_electronico { get; set; }
    }
}
