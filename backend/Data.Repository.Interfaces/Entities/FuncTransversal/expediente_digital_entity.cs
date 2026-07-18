using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.FuncTransversal
{
    public class expediente_digital_entity
    {
        [Key]
        public long id_archivo { get; set; }
        public long id_expediente { get; set; }
        public int id_documento { get; set; }
        public int? id_usuario { get; set; }        
        public string nombre_archivo { get; set; } = string.Empty;  
        public string nombre_archivo_original { get; set; } = string.Empty;
        public string extension { get; set; } = string.Empty;
        public int version_archivo { get; set; }
        public DateTime? fecha_alta { get; set; }
        public string comentarios { get; set; } = string.Empty;
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
