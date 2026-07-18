using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos
{
    public class interviniente_bbva_entity
    {
        [Key]
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_VALIDAR_INTEGRACION";
        public string nombre_completo { get; set; } = string.Empty;
        public string tipo_identificacion { get; set; } = string.Empty;
        public string numero_identificacion { get; set; } = string.Empty;
        public string? telefono { get; set; }
        public string? correo_electronico { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
