using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class gestion_rectificatoria_solucion_reparo : base_auditoria
    {
        public int id_gestion_rectificatoria_solucion_reparo { get; set; }
        public int id_gestion_rectificatoria { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool modificar_datos_memo { get; set; }
        public bool descontabilizar_operacion { get; set; }
        public bool subsanar { get; set; }
        public string? observaciones { get; set; }
        public int id_solicitante { get; set; }
        public int id_solicitud { get; set; }
        public string? solicitante { get; set; }
        public string? observaciones_reparo { get; set; }
        public DateTime? fecha_ingreso { get; set; }
    }
}
