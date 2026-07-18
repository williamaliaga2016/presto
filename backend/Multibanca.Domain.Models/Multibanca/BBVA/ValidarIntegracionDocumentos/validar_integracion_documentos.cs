using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca.BBVA.ValidarIntegracionDocumentos
{
    public class validar_integracion_documentos : base_auditoria
    {
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_VALIDAR_INTEGRACION";
        public bool? documentos_correctos { get; set; }
        public bool credito_condicionado { get; set; } = false;
        public string? motivo_devolucion { get; set; }
        public string? observaciones { get; set; }
    }
}
