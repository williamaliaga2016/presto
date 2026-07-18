using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class realizar_control_credito : base_auditoria
    {
        public int id_realizar_control_credito { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool enviar_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
