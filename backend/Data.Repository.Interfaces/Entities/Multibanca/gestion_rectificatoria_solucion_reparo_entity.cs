using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class gestion_rectificatoria_solucion_reparo_entity
    {
        [Key]
        public int id_gestion_rectificatoria_solucion_reparo { get; set; }
        public int id_gestion_rectificatoria { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool modificar_datos_memo {  get; set; }
        public bool descontabilizar_operacion {  get; set; }
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
