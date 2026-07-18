using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.FuncTransversal
{
    public class expediente_digital : base_auditoria
    {
        public long id_archivo { get; set; }
        public long id_expediente { get; set; }
        public int id_documento { get; set; }
        public int? id_usuario { get; set; }
        public string nombre_archivo { get; set; } = string.Empty;
        public string nombre_archivo_original { get; set; } = string.Empty;
        public string extension { get; set; } = string.Empty;
        public string? mime_type { get; set; }
        public long? file_size { get; set; }
        public int version_archivo { get; set; }
        public DateTime? fecha_alta { get; set; }
        public string comentarios { get; set; } = string.Empty;
        public string storage_provider { get; set; } = "local";
        public string storage_path { get; set; } = string.Empty;
        public string? activity_id { get; set; }
        public Dictionary<string, object>? metadata_extra { get; set; }
    }
}
