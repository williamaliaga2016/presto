using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class corregir_reparo_control_credito : base_auditoria
    {
        public int id_corregir_reparo_control_credito { get; set; }
        public int id_realizar_control_credito { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool subsanar { get; set; }
        public string? observaciones { get; set; }
        public int id_solicitud {  get; set; } 
        public int id_solicitante { get; set; }
        public string? solicitante { get; set; }
        public string? observaciones_reparo { get; set; }
        public DateTime? fecha_ingreso { get; set; }
    }
}
