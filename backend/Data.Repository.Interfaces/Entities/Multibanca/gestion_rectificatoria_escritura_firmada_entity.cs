using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class gestion_rectificatoria_escritura_firmada_entity
    {
        [Key]
        public int id_gestion_rectificatoria_escritura_firmada { get; set; }
        public int id_rectificatoria_firma_comprador_vendedor { get; set; }
        public long id_expediente { get; set; }
        public string? enviar_tipo_reparo { get; set; }
        public bool vb_solicitado_fiscalia { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool subsanar { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
