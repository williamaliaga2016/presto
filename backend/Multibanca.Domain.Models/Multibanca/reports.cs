using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca
{
    public class reports : base_auditoria
    {
        public int id_reporte { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string? descripcion { get; set; }
        public string report_path { get; set; } = string.Empty;
        public string? template { get; set; }
        public string? extension { get; set; }
    }
}
